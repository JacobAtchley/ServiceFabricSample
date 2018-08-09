using Grains.Interfaces;
using System;
using System.Threading.Tasks;
using Orleans.Client;

namespace OrleansStateless.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Run(args).Wait();
        }

        private static async Task Run(string[] args)
        {
            Console.WriteLine("Starting....");

            var client = OrleansClientFactory.Get(
                "fabric:/ServiceFabricSample/MyStatelessService",
                "UseDevelopmentStorage=true");

            Console.WriteLine("Connecting....");

            await client.Connect();

            Console.WriteLine("Connected");

            var grain = client.GetGrain<IMyFirstGrain>(Guid.Parse("26440F3A-D615-4DF9-9E55-A2E740B17C9B"));

            while (true)
            {
                var hello = await grain.SayHello();
                Console.WriteLine(hello);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
