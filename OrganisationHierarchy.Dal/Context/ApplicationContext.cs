using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrganisationHierarchy.Dal.Entities;
using OrganisationHierarchy.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganisationHierarchy.Dal.Context
{
    public partial class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<IdentityUser> IdentityUser { get; set; }

        public async Task<bool> SaveChangesAsync()
        {
            int changes = ChangeTracker
                          .Entries()
                          .Count(p => p.State == EntityState.Modified
                                   || p.State == EntityState.Deleted
                                   || p.State == EntityState.Added);

            if (changes == 0) return true;

            return await base.SaveChangesAsync() > 0;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Employee>()
                .HasOne(p => p.User)
               .WithMany()
               .HasForeignKey(p => p.Id);

            modelBuilder.Entity<Employee>()
                .HasOne(p => p.Position)
               .WithMany()
               .HasForeignKey(p => p.PositionId);

            modelBuilder.Entity<User>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<User>()
                .HasOne(p => p.IdentityUser)
               .WithMany()
               .HasForeignKey(p => p.Id);

        }
    }
}
