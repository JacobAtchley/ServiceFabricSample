using App.Core.Models.Entities;
using Fabric.Web.Hubs;
using Fabric.Web.Observers.Abstractions;
using Microsoft.AspNetCore.SignalR;
using System;

namespace Fabric.Web.Observers
{
    public class PeopleObserver : AbstractEntityObserver<Guid, Person>
    {
        /// <inheritdoc />
        public PeopleObserver(IHubContext<OrleansHub> hub) : base("Person", hub) { }
    }
}
