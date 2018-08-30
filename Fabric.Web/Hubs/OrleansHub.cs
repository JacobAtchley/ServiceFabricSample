using Microsoft.AspNetCore.SignalR;
using Oreleans.Observers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fabric.Web.Hubs
{
    public class OrleansHub : Hub
    {
        private static readonly object Key = new object();

        private readonly IEnumerable<IOrleansSubscriber> _subscribers;
        private static volatile int _clientCount;

        /// <summary>
        /// This class resolves all the IOrleansSubscribers from
        ///  the IoC container connects them to Signal R only once.
        ///  The Key object is used to ensure we only register or
        /// unregister the subscribers once.
        /// </summary>
        /// <param name="subscribers">
        /// The <see cref="IEnumerable{T}"/> of <see cref="IOrleansSubscriber"/>
        /// subscribers to register</param>
        public OrleansHub(IEnumerable<IOrleansSubscriber> subscribers)
        {
            _subscribers = subscribers;
        }

        public override async Task OnConnectedAsync()
        {
            bool initClient;

            lock (Key)
            {
                _clientCount++;
                initClient = _clientCount == 1;
            }

            if (initClient)
            {
                await Task.WhenAll(_subscribers.Select(x => x.InitClientAsync()));
            }

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var unsubscribe = false;

            lock (Key)
            {
                _clientCount--;

                if (_clientCount < 1)
                {
                    unsubscribe = true;
                }
            }

            if (unsubscribe)
            {
                foreach (var subscriber in _subscribers)
                {
                    subscriber.Unsubscribe();
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}