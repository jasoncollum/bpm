using System;
using System.Collections.Generic;
using System.Text;
using bpm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bpm.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Entry> Entry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            modelBuilder.Entity<Entry>()
                .Property(r => r.DateEntered)
                .HasDefaultValueSql("GETDATE()");

            ApplicationUser user = new ApplicationUser
            {
                FirstName = "Jason",
                LastName = "Collum",
                UserName = "jason@email.com",
                NormalizedUserName = "JASON@EMAIL.COM",
                Email = "jasony@email.com",
                NormalizedEmail = "JASON@EMAIL.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                Id = "00000000-ffff-ffff-ffff-ffffffffffff"
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Password1*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            modelBuilder.Entity<Entry>().HasData(
               new Entry()
               {
                   Id = 1,
                   Systolic = 128,
                   Diastolic = 65,
                   Pulse = 65,
                   Weight = 140,
                   Notes = "Took morning meds at 8:00am",
                   ApplicationUserId = "00000000-ffff-ffff-ffff-ffffffffffff"
               },
               new Entry()
               {
                   Id = 2,
                   Systolic = 135,
                   Diastolic = 61,
                   Pulse = 68,
                   Weight = 140,
                   Notes = "Another note",
                   ApplicationUserId = "00000000-ffff-ffff-ffff-ffffffffffff"
               },
               new Entry()
               {
                   Id = 3,
                   Systolic = 140,
                   Diastolic = 67,
                   Pulse = 68,
                   Weight = 140,
                   Notes = "And Another note",
                   ApplicationUserId = "00000000-ffff-ffff-ffff-ffffffffffff"
               }
             );
        }
    }
}
