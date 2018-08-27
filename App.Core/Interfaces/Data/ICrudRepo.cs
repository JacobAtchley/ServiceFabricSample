using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.Interfaces.Data
{
    public interface ICrudRepo<in TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {
        Task<TEntity> GetByKeyAsync(TKey key, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> UpdateAsync(TKey key, TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(TKey key, CancellationToken cancellationToken);
    }
}
