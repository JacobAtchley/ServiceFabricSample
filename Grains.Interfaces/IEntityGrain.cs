using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Grains.Interfaces.Observers;
using Orleans;
using System.Threading.Tasks;

namespace Grains.Interfaces
{
    public interface IEntityGrain<TKey, TEntity>
        : IGrainWithGuidKey, ICrudRepo<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {
        Task Subscribe(IEntityModifiedObserver<TKey, TEntity> observer);
    }
}
