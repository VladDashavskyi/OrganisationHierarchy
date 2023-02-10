using Microsoft.EntityFrameworkCore;
using OrganisationHierarchy.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganisationHierarchy.Dal.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<User> User { get; set; }
        DbSet<Position> Position { get; set; }
        DbSet<Employee> Employee { get; set; }
        DbSet<IdentityUser> IdentityUser { get; set; }
        
        Task<bool> SaveChangesAsync();
    }
}
