using IdentityDemoNet3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityDemoNet3.Usuario;

namespace IdentityDemoNet3
{
    public class IdentityDemoUserDbContext:IdentityDbContext<Usuario>
    {

        public IdentityDemoUserDbContext(DbContextOptions<IdentityDemoUserDbContext> options):base(options)
        {

        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Usuario>(user => user.HasIndex(x => x.Locale).IsUnique(false));
            builder.Entity<Organization>(org =>
            {
                org.ToTable("Organizations");
                org.HasKey(x=> x.Id);
                org.HasMany<Usuario>().WithOne().HasForeignKey(x => x.OrgId).IsRequired(false);
            }
           );

        }
    }
}
