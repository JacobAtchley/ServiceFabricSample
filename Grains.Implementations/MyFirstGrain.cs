using Grains.Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grains.Implementations
{
    public class MyFirstGrain : Grain, IMyFirstGrain
    {
        /// <inheritdoc />
        public Task<string> SayHello()
        {
            return Task.FromResult($"Hello I'm the {nameof(IMyFirstGrain)} implementation. The current date and time is {DateTime.UtcNow:R}");
        }
    }
}
