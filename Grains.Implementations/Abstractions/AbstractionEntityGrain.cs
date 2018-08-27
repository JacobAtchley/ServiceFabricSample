using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Grains.Interfaces;
using Orleans;
using System.Collections.Generic;
using System.Threading;
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
        public Task<TEntity> GetByKeyAsync(TKey key, CancellationToken cancellationToken)
        {
            return _crudRepo.GetByKeyAsync(key, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _crudRepo.GetAllAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return _crudRepo.AddAsync(entity, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TEntity> UpdateAsync(TKey key, TEntity entity, CancellationToken cancellationToken)
        {
            return _crudRepo.UpdateAsync(key, entity, cancellationToken);
        }

        /// <inheritdoc />
        public Task DeleteAsync(TKey key, CancellationToken cancellationToken)
        {
            return _crudRepo.DeleteAsync(key, cancellationToken);
        }
    }
}
