using Grains.Interfaces;
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
        public Task<string> SayHello()
        {
            var message = $"Hello I'm the {nameof(IMyFirstGrain)} implementation. The current date and time is {DateTime.UtcNow:R}";
            _observers.Notify(observer => observer.MessageUpdated(message));
            return Task.FromResult(message);
        }

        public Task Subscribe(IHelloObserver observer)
        {
            _observers.Subscribe(observer);
            return Task.CompletedTask;
        }
    }
}
