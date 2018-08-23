using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Grains.Interfaces;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grains.Implementations.Abstractions
{
    public class AbstractionEntityGrain<TKey, TEntity>
        : Grain, IEntityGrain<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly ICrudRepo<TKey, TEntity> _crudRepo;

        public AbstractionEntityGrain(ICrudRepo<TKey, TEntity> crudRepo)
        {
            _crudRepo = crudRepo;
        }

        /// <inheritdoc />
        public Task<TEntity> GetByKeyAsync(TKey key)
        {
            return _crudRepo.GetByKeyAsync(key);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return _crudRepo.GetAllAsync();
        }

        /// <inheritdoc />
        public Task<TEntity> AddAsync(TEntity entity)
        {
            return _crudRepo.AddAsync(entity);
        }

        /// <inheritdoc />
        public Task<TEntity> UpdateAsync(TKey key, TEntity entity)
        {
            return _crudRepo.UpdateAsync(key, entity);
        }

        /// <inheritdoc />
        public Task DeleteAsync(TKey key)
        {
            return _crudRepo.DeleteAsync(key);
        }
    }
}
