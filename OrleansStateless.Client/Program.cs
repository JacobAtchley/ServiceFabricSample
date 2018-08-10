using Grains.Interfaces;
using Oreleans.Observers;
using Orleans.Client;
using System;
using System.Threading.Tasks;

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


            var grain = client.GetGrain<IMyFirstGrain>(Guid.Empty);

            var subscriber = new HelloSubscriber(new ClientHelloObserver());

            await subscriber.InitClientAsync();

            Console.WriteLine("Connected. Type a messge to send. Type quit to exit.");

            string message;

            do
            {
                message = Console.ReadLine();

                if (message != "quit")
                {
                    await grain.ChatAsync(message);
                }
            }
            while (message != "quit");
        }
    }
}
