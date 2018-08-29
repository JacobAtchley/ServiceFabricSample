using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Grains.Interfaces;
using Grains.Interfaces.Observers;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Grains.Implementations.Abstractions
{
    public class AbstractionEntityGrain<TKey, TEntity>
        : Grain, IEntityGrain<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {

        private readonly GrainObserverManager<IEntityModifiedObserver<TKey, TEntity>> _observers = new GrainObserverManager<IEntityModifiedObserver<TKey, TEntity>>
        {
            ExpirationDuration = TimeSpan.FromSeconds(5)
        };

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
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var result = await _crudRepo.AddAsync(entity, cancellationToken);
            _observers.Notify(observer => observer.Modified("Added", result));
            return result;
        }

        /// <inheritdoc />
        public async Task<TEntity> UpdateAsync(TKey key, TEntity entity, CancellationToken cancellationToken)
        {
            var result = await _crudRepo.UpdateAsync(key, entity, cancellationToken);
            _observers.Notify(observer => observer.Modified("Updated", result));
            return result;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(TKey key, CancellationToken cancellationToken)
        {
            await _crudRepo.DeleteAsync(key, cancellationToken);
            _observers.Notify(observer => observer.Modified("Deleted", null));
        }

        public Task Subscribe(IEntityModifiedObserver<TKey, TEntity> observer)
        {
            _observers.Subscribe(observer);

            return Task.CompletedTask;
        }
    }
}
