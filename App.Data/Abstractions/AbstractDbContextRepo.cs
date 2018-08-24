﻿using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using App.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Data.Abstractions
{
    public class AbstractDbContextRepo<TKey, TEntity> : ICrudRepo<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
        where TKey : struct
    {
        private readonly IAppDbContext _context;

        public AbstractDbContextRepo(IAppDbContext context)
        {
            _context = context;
        }

        private DbSet<TEntity> GetDbSet()
        {
            return _context.Set<TEntity>();
        }

        /// <inheritdoc />
        public Task<TEntity> GetByKeyAsync(TKey key)
        {
            return GetDbSet().FindAsync(key);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await GetDbSet().ToArrayAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var e = (await GetDbSet().AddAsync(entity).ConfigureAwait(false)).Entity;

            await _context.SaveChangesAsync(CancellationToken.None);

            return e;
        }

        /// <inheritdoc />
        public async Task<TEntity> UpdateAsync(TKey key, TEntity entity)
        {
            entity.Id = key;
            var entry = GetDbSet().Attach(entity);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync(CancellationToken.None);

            return entry.Entity;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(TKey key)
        {
            var entry = GetDbSet().Attach(new TEntity
            {
                Id = key
            });

            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}