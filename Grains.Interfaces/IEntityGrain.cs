using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Grains.Interfaces.Observers;

namespace Grains.Interfaces
{
    public interface IEntityGrain<TKey, TEntity>
        : ICrudRepo<TKey, TEntity>,
            ISubscribeGrain<IEntityModifiedObserver<TKey, TEntity>>
        where TEntity : class, IEntity<TKey>, new()
    {
    }
}
