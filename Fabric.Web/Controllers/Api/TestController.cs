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
        public async Task<IActionResult> Get()
        {
            var client = OrleansClientFactory.Get(
                "fabric:/ServiceFabricSample/MyStatelessService",
                "UseDevelopmentStorage=true");

            await client.Connect();

            Console.WriteLine("Connected");

            var grain = client.GetGrain<IMyFirstGrain>(Guid.Parse("0821E848-404D-4228-A647-D772480820AE"));

            var hello = await grain.SayHello();

            return Ok(hello);
        }
    }
}
