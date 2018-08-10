using Orleans;

namespace Grains.Interfaces
{
    public interface IHelloObserver : IGrainObserver
    {
        void MessageUpdated(string messge);
    }
}