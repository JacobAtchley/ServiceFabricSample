using Microsoft.AspNetCore.SignalR;
using Oreleans.Observers.Interfaces;
using System;
using System.Threading.Tasks;

namespace Fabric.Web.Hubs
{
    public class OrleansHub : Hub
    {
        private static readonly object Key = new object();

        private readonly IHelloSubscriber _subscriber;
        private static volatile int _clientCount;

        public OrleansHub(IHelloSubscriber subscriber)
        {
            _subscriber = subscriber;
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
                await _subscriber.InitClientAsync();
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
                _subscriber.Unsubscribe();
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}