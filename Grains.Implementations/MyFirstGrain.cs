using Grains.Interfaces;
using Grains.Interfaces.Observers;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grains.Implementations
{
    public class MyFirstGrain : Grain, IMyFirstGrain
    {
        private readonly GrainObserverManager<IHelloObserver> _observers = new GrainObserverManager<IHelloObserver>
        {
            ExpirationDuration = TimeSpan.FromSeconds(5)
        };

        /// <inheritdoc />
        public Task ChatAsync(string message)
        {
            _observers.Notify(observer => observer.MessageUpdated(message));

            return Task.CompletedTask;
        }

        public Task Subscribe(IHelloObserver observer)
        {
            _observers.Subscribe(observer);

            return Task.CompletedTask;
        }
    }
}
