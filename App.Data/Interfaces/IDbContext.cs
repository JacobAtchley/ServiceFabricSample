using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace App.Data.Interfaces
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        int SaveChanges();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
