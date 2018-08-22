using App.Core.Interfaces;
using System;

namespace App.Core.Models
{
    public class Person : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public DateTimeOffset DateCreated { get; set; }
    }
}
