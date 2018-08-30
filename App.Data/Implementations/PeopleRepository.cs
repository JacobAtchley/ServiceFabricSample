using App.Core.Models.Entities;
using App.Data.Abstractions;
using App.Data.Interfaces;
using System;

namespace App.Data.Implementations
{
    public class PeopleRepository : AbstractDbContextRepo<Guid, Person>
    {
        /// <inheritdoc />
        public PeopleRepository(IAppDbContext context) : base(context)
        {

        }
    }
}
