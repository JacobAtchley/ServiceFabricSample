using App.Core.Models.Entities;
using Fabric.Web.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;

namespace Fabric.Web.Controllers.Api
{
    [Route("/api/grains/people")]
    public class PeopleGrainController : AbstractEntityGrainController<Guid, Person>
    {
        /// <inheritdoc />
        public PeopleGrainController(IClusterClient clusterClient) : base(clusterClient)
        {

        }
    }
}
