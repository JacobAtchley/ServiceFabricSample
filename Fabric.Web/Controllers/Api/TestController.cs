using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;

namespace Fabric.Web.Controllers.Api
{
    [Route("/api/[controller]")]
    public class TestController : Controller
    {
        private readonly IClusterClient _clusterClient;
        public TestController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"Hello from the server! The current date and time is {DateTime.UtcNow:R}");
        }
    }
}