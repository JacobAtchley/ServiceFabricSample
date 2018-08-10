using System;
using System.Threading.Tasks;
using Fabric.Web.Observers;
using Grains.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Orleans.Client;

namespace Fabric.Web.Hubs
{
    public class OrleansHub : Hub
    {
        private readonly IHelloSubscriber _subscriber;
        private static volatile int _clientCount = 0;
        private static readonly object _key = new object();  

        public OrleansHub(IHelloSubscriber subscriber)
        {
            _subscriber = subscriber;
        }

        public override async Task OnConnectedAsync()
        {
            var initClient = false;
            lock (_key)
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
            lock (_key)
            {
                _clientCount--;
                if (_clientCount < 1)
                    unsubscribe = true;
            }

            if (unsubscribe)
            {
                _subscriber.Unsubscribe();
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}