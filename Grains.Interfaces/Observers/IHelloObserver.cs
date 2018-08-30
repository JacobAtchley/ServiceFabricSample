using Orleans;

namespace Grains.Interfaces.Observers
{
    public interface IHelloObserver : IGrainObserver
    {
        void MessageUpdated(string messge);
    }
}