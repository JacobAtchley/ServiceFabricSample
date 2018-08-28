using App.Core.Models;
using Grains.Interfaces;
using Orleans.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Oreleans.Observers
{
    public interface IHelloSubscriber
    {
        Task InitClientAsync();
        void Unsubscribe();
    }

    public class HelloSubscriber : IHelloSubscriber
    {
        private readonly IHelloObserver _observer;
        private readonly OrleansClientConnectionOptions _options;
        private IMyFirstGrain _grain;
        private IHelloObserver _observerReference;
        private CancellationTokenSource _cancellationToken;

        public HelloSubscriber(IHelloObserver observer, OrleansClientConnectionOptions options)
        {
            _observer = observer;
            _options = options;
        }

        public async Task InitClientAsync()
        {
            var client = OrleansClientFactory.Get(_options);

            await client.Connect();

            Console.WriteLine("Connected");

            _grain = client.GetGrain<IMyFirstGrain>(Guid.Empty);

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