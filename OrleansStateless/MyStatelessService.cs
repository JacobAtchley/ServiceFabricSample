using Fabric.Orleans;
using Fabric.Web;
using Fabric.Web.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
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
            var settings = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings;

            return new[]
            {
                OrleansServiceInstanceListenerFactory.Get(settings),

                WebClientFactory.Get(new WebClientFactoryOptions
                {
                    ContentDirectory = (new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.FullName ?? string.Empty) + "\\Fabric.Web",
                    EndpointName = "Web",
                    LogAction = (ctx, log) => ServiceEventSource.Current.ServiceMessage(ctx, log),
                    FabricConfiguration = settings
                })
            };
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(Context, $"I'm Still Alive! {DateTime.UtcNow:R}");

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}
