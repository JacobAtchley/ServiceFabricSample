using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Core.Interfaces.Data
{
    public interface ICrudRepo<in TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {
        Task<TEntity> GetByKeyAsync(TKey key);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TKey key, TEntity entity);

        Task DeleteAsync(TKey key);
    }
}
