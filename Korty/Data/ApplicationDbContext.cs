using Korty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Korty.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Court> Courts { get; set; } = default!;
        public DbSet<Reservation> Reservations { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN" }
            );

            // Seed Courts
            modelBuilder.Entity<Court>().HasData(
                new Court { Id = 1, Name = "Kort 1", Description = "Kort tenisowy z nawierzchnią ceglaną", IsActive = true, OpeningHour = new TimeOnly(6, 0, 0), ClosingHour = new TimeOnly(23, 0, 0) },
                new Court { Id = 2, Name = "Kort 2", Description = "Kort tenisowy z nawierzchnią trawiastą", IsActive = true, OpeningHour = new TimeOnly(6, 0, 0), ClosingHour = new TimeOnly(23, 0, 0) },
                new Court { Id = 3, Name = "Kort 3", Description = "Kort tenisowy z nawierzchnią twardą", IsActive = true, OpeningHour = new TimeOnly(6, 0, 0), ClosingHour = new TimeOnly(23, 0, 0) }
            );
        }
    }
}