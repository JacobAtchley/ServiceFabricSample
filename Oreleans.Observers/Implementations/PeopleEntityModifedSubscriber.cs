using App.Core.Models;
using App.Core.Models.Entities;
using Grains.Interfaces.Grains;
using Grains.Interfaces.Observers;
using Oreleans.Observers.Abstractions;
using System;

namespace Oreleans.Observers.Implementations
{
    public class PeopleEntityModifedSubscriber : AbstractOrleansSubscriber<IEntityGrain<Guid, Person>, IEntityModifiedObserver<Guid, Person>>
    {
        /// <inheritdoc />
        public PeopleEntityModifedSubscriber(IEntityModifiedObserver<Guid, Person> observer, OrleansClientConnectionOptions options) : base(observer, options) { }
    }
}
