using Grains.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Hosting.ServiceFabric;
using System.Fabric;

namespace OrleansStateless
{
    public static class OrleansServiceInstanceListenerFactory
    {
        public static ServiceInstanceListener Get()
        {
            return OrleansServiceListener.CreateStateless(Configure);
        }

        private static void Configure(StatelessServiceContext context, ISiloHostBuilder builder)
        {
            builder.Configure<ClusterOptions>(options =>
            {
                options.ServiceId = context.ServiceName.ToString();
                options.ClusterId = "development";
            });

            builder.UseAzureStorageClustering(options => options.ConnectionString = "UseDevelopmentStorage=true");

            builder.ConfigureLogging(logging => logging.AddDebug());

            var activation = context.CodePackageActivationContext;
            var endpoints = activation.GetEndpoints();

            var siloEndpoint = endpoints["OrleansSiloEndpoint"];
            var gatewayEndpoint = endpoints["OrleansProxyEndpoint"];
            var hostname = context.NodeContext.IPAddressOrFQDN;

            builder.ConfigureEndpoints(hostname, siloEndpoint.Port, gatewayEndpoint.Port);

            builder.ConfigureApplicationParts(parts =>
            {
                parts.AddApplicationPart(typeof(MyFirstGrain).Assembly).WithReferences();
                parts.AddFromApplicationBaseDirectory();
            });
        }
    }
}
