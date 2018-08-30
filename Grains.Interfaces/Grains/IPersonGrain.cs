using App.Core.Models.Entities;
using System;

namespace Grains.Interfaces.Grains
{
    //todo: figure out why i have to do this. 
    //if its not included serialization errors occur in Orleans.
    //look into known type assembly attribute
    public interface IPersonGrain : IEntityGrain<Guid, Person>
    {
    }
}
