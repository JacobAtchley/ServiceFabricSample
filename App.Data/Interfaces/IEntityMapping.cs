using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Interfaces
{
    public interface IEntityMapping<T> where T : class
    {
        void Map(EntityTypeBuilder<T> entityTypeConfiguration);
    }
}
