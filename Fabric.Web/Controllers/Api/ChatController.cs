using Fabric.Web.Abstractions;
using Fabric.Web.Models;
using Grains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Fabric.Web.Controllers.Api
{
    [Route("/api/[controller]")]
    public class ChatController : AbstractGrainClientController<IMyFirstGrain>
    {
        /// <inheritdoc />
        public ChatController(IClusterClient clusterClient) : base(clusterClient)
        {

        }

        [HttpPost]
        public async Task<IActionResult> GetOrleansAsync([FromBody]MessageViewModel message)
        {
            await UseGrain(Guid.Empty, x => x.ChatAsync(message.Message));

            return Ok("Message Processed");
        }
    }
}
