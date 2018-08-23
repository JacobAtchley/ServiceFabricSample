using App.Core.Models;
using System;

namespace Grains.Interfaces
{
    public interface IPeopleGrain : IEntityGrain<Guid, Person>
    {
    }
}
