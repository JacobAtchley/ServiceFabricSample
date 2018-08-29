using App.Core.Interfaces;
using Orleans;

namespace Grains.Interfaces.Observers
{
    public interface IEntityModifiedObserver<TKey, in TEntity> : IGrainObserver
        where TEntity : class, IEntity<TKey>, new()
    {
        void Modified(string reason, TEntity entity);
    }
}
