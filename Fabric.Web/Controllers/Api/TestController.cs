using Grains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans.Client;
using System;
using System.Threading.Tasks;

namespace Fabric.Web.Controllers.Api
{
    [Route("/api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet("orleans")]
        public async Task<IActionResult> GetOrleansAsync()
        {
            var client = OrleansClientFactory.Get(
                "fabric:/ServiceFabricSample/MyStatelessService",
                "UseDevelopmentStorage=true");

            await client.Connect();

            Console.WriteLine("Connected");
            
            var grain = client.GetGrain<IMyFirstGrain>(Guid.Parse("26440F3A-D615-4DF9-9E55-A2E740B17C9B"));

            var hello = await grain.SayHello();

            return Ok(hello);
        }

        public IActionResult Get()
        {
            return Ok($"Hello from the server! The current date and time is {DateTime.UtcNow:R}");
        }
    }
}