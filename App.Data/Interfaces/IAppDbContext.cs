using App.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Interfaces
{
    public interface IAppDbContext : IDbContext
    {
        DbSet<Person> People { get; set; }
    }
}
