using App.Core.Models;
using App.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace App.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly IAppContextSettings _settings;

        public AppDbContext(IAppContextSettings settings, ILogger<AppDbContext> logger)
        {
            _settings = settings;
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_settings.ConnectionString);
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var interfaceType = typeof(IEntityMapping<>);

            var mappings = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.Namespace == "App.Data.Mappings")
                .Select(x => new
                {
                    mappingType = x,
                    entityType = x.GetInterfaces()
                        .Where(y => y.IsGenericType)
                        .Where(y => interfaceType.IsAssignableFrom(y.GetGenericTypeDefinition()))
                        .SelectMany(y => y.GetGenericArguments())
                        .FirstOrDefault()
                })
                .Where(x => x.entityType != null)
                .ToArray();

            var addMapping = GetType().GetMethod(nameof(AddMapping), BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var mapping in mappings)
            {
                var addMappingEntity = addMapping.MakeGenericMethod(mapping.mappingType, mapping.entityType);
                addMappingEntity.Invoke(this, new object[] { modelBuilder });
            }

            base.OnModelCreating(modelBuilder);

        }

        private void AddMapping<TMapping, TEntity>(ModelBuilder modelBuilder) where TEntity : class
        {
            var mapping = Activator.CreateInstance<TMapping>() as IEntityMapping<TEntity>;
            mapping?.Map(modelBuilder.Entity<TEntity>());
        }

        /// <inheritdoc />
        public DbSet<Person> People { get; set; }
    }
}
