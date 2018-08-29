using Orleans;
using System.Threading.Tasks;

namespace Grains.Interfaces
{
    public interface ISubscribeGrain<in TObserver> : IGrainWithGuidKey
    {
        Task Subscribe(TObserver observer);
    }
}
