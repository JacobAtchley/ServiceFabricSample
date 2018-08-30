using App.Core.Interfaces;
using Orleans;

namespace Grains.Interfaces.Observers
{
    /// <summary>
    /// Provides methods for notifying grain observers an entity has changed. 
    /// </summary>
    /// <typeparam name="TKey">The Entity's Key</typeparam>
    /// <typeparam name="TEntity">The Entity's Type</typeparam>
    public interface IEntityModifiedObserver<TKey, in TEntity> : IGrainObserver
        where TEntity : class, IEntity<TKey>, new()
    {
        void Modified(string reason, TEntity entity);
    }
}
