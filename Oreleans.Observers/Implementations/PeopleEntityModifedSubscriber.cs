﻿using App.Core.Models;
using Grains.Interfaces;
using Grains.Interfaces.Observers;
using Oreleans.Observers.Abstractions;
using System;

namespace Oreleans.Observers.Implementations
{
    public class PeopleEntityModifedSubscriber : AbstractEntityModifiedSubscriber<Guid, Person, IPeopleGrain>
    {
        /// <inheritdoc />
        public PeopleEntityModifedSubscriber(IEntityModifiedObserver<Guid, Person> observer, OrleansClientConnectionOptions options) : base(observer, options) { }
    }
}
