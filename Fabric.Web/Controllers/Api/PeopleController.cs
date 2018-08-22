using App.Core.Interfaces.Data;
using App.Core.Models;
using Fabric.Web.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Fabric.Web.Controllers.Api
{

    [Route("/api/[controller]")]
    public class PeopleController : AbstractCrudController<Guid, Person>
    {
        /// <inheritdoc />
        public PeopleController(ICrudRepo<Guid, Person> crudRepo) : base(crudRepo)
        {

        }
    }
}
