using App.Core.Interfaces;
using App.Core.Interfaces.Data;
using Orleans;

namespace Grains.Interfaces
{
    public interface IEntityGrain<in TKey, TEntity>
        : IGrainWithGuidKey, ICrudRepo<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {
    }
}
