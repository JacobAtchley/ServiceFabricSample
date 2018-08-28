using Fabric.Core;
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
            var myConfigPackage = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");

            var fabricSettings = new FabricSettings(myConfigPackage);

            var webFactoryOptions = new WebClientFactoryOptions
            {
                FabricSettings = fabricSettings,
                EndpointName = "Web",
                LogAction = (ctx, log) => ServiceEventSource.Current.ServiceMessage(ctx, log),
                ContentRoot = (new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.FullName ?? string.Empty) + "\\Fabric.Web"
            };

            return new[]
            {
                OrleansServiceInstanceListenerFactory.Get(fabricSettings),

                WebClientFactory.Get(webFactoryOptions)
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
