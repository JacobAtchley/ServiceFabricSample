using System;
using System.Threading;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans.Client;

namespace Fabric.Web.Observers
{
    public interface IHelloSubscriber
    {
        Task InitClientAsync();
        void Unsubscribe();
    }

    public class HelloSubscriber : IHelloSubscriber
    {
        private readonly IHelloObserver _observer;
        private IMyFirstGrain _grain;
        private IHelloObserver _observerReference;
        private CancellationTokenSource _cancellationToken;

        public HelloSubscriber(IHelloObserver observer)
        {
            _observer = observer;
        }

        public async Task InitClientAsync()
        {
            var client = OrleansClientFactory.Get(
                "fabric:/ServiceFabricSample/MyStatelessService",
                "UseDevelopmentStorage=true");

            await client.Connect();

            Console.WriteLine("Connected");
            
            _grain = client.GetGrain<IMyFirstGrain>(Guid.Parse("26440F3A-D615-4DF9-9E55-A2E740B17C9B"));

            _observerReference = await client.CreateObjectReference<IHelloObserver>(_observer);
            _cancellationToken = new CancellationTokenSource();
            StaySubscribed(_cancellationToken.Token);
        }

        public void Unsubscribe()
        {
            if (_cancellationToken != null && !_cancellationToken.IsCancellationRequested)
                _cancellationToken.Cancel();
        }

        private async void StaySubscribed(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await _grain.Subscribe(_observerReference);
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception while trying to subscribe for updates: {exception}");
                }
            }
        }
    }
}