using Fabric.Orleans;
using Fabric.Web;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace MyStatelessService
{
    internal sealed class MyStatelessService : StatelessService
    {
        public MyStatelessService(StatelessServiceContext context)
            : base(context)
        {

        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                OrleansServiceInstanceListenerFactory.Get(),

                WebClientFactory.Get("Web", (context, log) =>
                    ServiceEventSource.Current.ServiceMessage(context, log))
            };
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
