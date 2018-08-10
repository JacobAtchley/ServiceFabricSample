using Grains.Interfaces;
using System;

namespace OrleansStateless.Client
{
    public class ClientHelloObserver : IHelloObserver
    {
        /// <inheritdoc />
        public void MessageUpdated(string messge)
        {
            Console.WriteLine($"{nameof(ClientHelloObserver)} - {messge} ");
        }
    }
}
