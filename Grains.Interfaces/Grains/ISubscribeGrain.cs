using Orleans;
using System.Threading.Tasks;

namespace Grains.Interfaces.Grains
{
    public interface ISubscribeGrain<in TObserver> : IGrainWithGuidKey
    {
        Task Subscribe(TObserver observer);
    }
}
