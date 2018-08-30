using App.Core.Models;
using Grains.Interfaces.Grains;
using Oreleans.Observers.Interfaces;
using Orleans;
using Orleans.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Oreleans.Observers.Abstractions
{
    /// <summary>
    /// Scaffolds the process for subscribing to a grain.
    /// </summary>
    /// <typeparam name="TGrain">The type of grain to observe</typeparam>
    /// <typeparam name="TObServer">Tye grain observer type</typeparam>
    public class AbstractOrleansSubscriber<TGrain, TObServer>
        : IOrleansSubscriber
        where TGrain : ISubscribeGrain<TObServer>
        where TObServer : IGrainObserver
    {
        private readonly TObServer _observer;
        private readonly OrleansClientConnectionOptions _options;
        private TGrain _grain;
        private TObServer _observerReference;
        private CancellationTokenSource _cancellationToken;

        public AbstractOrleansSubscriber(TObServer observer, OrleansClientConnectionOptions options)
        {
            _observer = observer;
            _options = options;
        }

        public async Task InitClientAsync()
        {
            var client = OrleansClientFactory.Get(_options);

            await client.Connect();

            _grain = client.GetGrain<TGrain>(Guid.Empty);

            _observerReference = await client.CreateObjectReference<TObServer>(_observer);
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
