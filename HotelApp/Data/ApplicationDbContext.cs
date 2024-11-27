using HotelApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HotelApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
            public DbSet<RoomType> RoomTypes { get; set; }
            public DbSet<Amenity> Amenities { get; set; }
            public DbSet<Room> Rooms { get; set; }
            public DbSet<Voucher> Vouchers { get; set; }
            public DbSet<Image> Images { get; set; }
            public DbSet<Area> Areas { get; set; }
            public DbSet<Contact> Contacts { get; set; }
            public DbSet<AppUser> AppUsers { get; set; }
            public DbSet<Booking> Bookings { get; set; }
            public DbSet<CCCD> CCCDs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu cho bảng AspNetRoles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(), 
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Client",
                    NormalizedName = "CLIENT"
                }
            );
        }

    }
}
