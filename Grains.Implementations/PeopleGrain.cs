using App.Core.Interfaces.Data;
using App.Core.Models;
using Grains.Implementations.Abstractions;
using System;

namespace Grains.Implementations
{
    public class PeopleGrain : AbstractionEntityGrain<Guid, Person>
    {
        /// <inheritdoc />
        public PeopleGrain(ICrudRepo<Guid, Person> crudRepo) : base(crudRepo)
        {

        }
    }
}
