using Orleans;
using System.Threading.Tasks;

namespace Grains.Interfaces
{
    public interface IMyFirstGrain : IGrainWithGuidKey
    {
        Task<string> SayHello();
        Task Subscribe(IHelloObserver observer);
    }
}
