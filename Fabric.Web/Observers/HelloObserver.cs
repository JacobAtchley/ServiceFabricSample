using Fabric.Web.Hubs;
using Grains.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Fabric.Web.Observers
{
    public class HelloObserver : IHelloObserver
    {
        private readonly IHubContext<OrleansHub> _hub;

        public HelloObserver(IHubContext<OrleansHub> hub)
        {
            _hub = hub;
        }

        public void MessageUpdated(string message)
        {
            _hub.Clients.All.SendAsync("Hello", message);
        }
    }
}