using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Grains.Interfaces.Observers;

namespace Grains.Interfaces.Grains
{
    /// <summary>
    /// Provides functionality for CRUD'ing an entity 
    /// </summary>
    /// <typeparam name="TKey">The Entity's Key</typeparam>
    /// <typeparam name="TEntity">The Entity's Type</typeparam>
    public interface IEntityGrain<TKey, TEntity>
        : ICrudRepo<TKey, TEntity>,
            ISubscribeGrain<IEntityModifiedObserver<TKey, TEntity>>
        where TEntity : class, IEntity<TKey>, new()
    {
    }
}
