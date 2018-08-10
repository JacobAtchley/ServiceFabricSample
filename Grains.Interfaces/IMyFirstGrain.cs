using Orleans;
using System.Threading.Tasks;

namespace Grains.Interfaces
{
    public interface IMyFirstGrain : IGrainWithGuidKey
    {
        Task ChatAsync(string message);

        Task Subscribe(IHelloObserver observer);
    }
}
