using App.Core.Interfaces;
using App.Core.Models;
using Grains.Interfaces;
using Grains.Interfaces.Observers;
using Oreleans.Observers.Interfaces;
using Orleans.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Oreleans.Observers.Abstractions
{
    public abstract class AbstractEntityModifiedSubscriber<TKey, TEntity, TGrain>
        : IEntityModifiedSubscriber
        where TEntity : class, IEntity<TKey>, new()
        where TGrain : IEntityGrain<TKey, TEntity>
    {
        private readonly IEntityModifiedObserver<TKey, TEntity> _observer;
        private readonly OrleansClientConnectionOptions _options;

        private TGrain _grain;
        private IEntityModifiedObserver<TKey, TEntity> _observerReference;
        private CancellationTokenSource _cancellationToken;

        public AbstractEntityModifiedSubscriber(
            IEntityModifiedObserver<TKey, TEntity> observer,
            OrleansClientConnectionOptions options)
        {
            _observer = observer;
            _options = options;
        }

        public async Task InitClientAsync()
        {
            var client = OrleansClientFactory.Get(_options);

            await client.Connect();

            _grain = client.GetGrain<TGrain>(Guid.Empty);

            _observerReference = await client.CreateObjectReference<IEntityModifiedObserver<TKey, TEntity>>(_observer);

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
