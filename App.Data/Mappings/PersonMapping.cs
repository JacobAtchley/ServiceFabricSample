using App.Core.Models;
using App.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Mappings
{
    public class PersonMapping : IEntityMapping<Person>
    {
        /// <inheritdoc />
        public void Map(EntityTypeBuilder<Person> entityTypeConfiguration)
        {
            entityTypeConfiguration.HasKey(x => x.Id);
        }
    }
}
