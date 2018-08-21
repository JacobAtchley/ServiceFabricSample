using Fabric.Web.Models;
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
        [HttpPost("chat")]
        public async Task<IActionResult> GetOrleansAsync([FromBody]MessageViewModel message)
        {
            var client = OrleansClientFactory.Get(
                "fabric:/ServiceFabricSample/MyStatelessService",
                "UseDevelopmentStorage=true");

            await client.Connect();

            Console.WriteLine("Connected");

            var grain = client.GetGrain<IMyFirstGrain>(Guid.Empty);

            await grain.ChatAsync(message.Message);

            return Ok("Message Processed");
        }

        public IActionResult Get()
        {
            return Ok($"Hello from the server! The current date and time is {DateTime.UtcNow:R}");
        }
    }
}