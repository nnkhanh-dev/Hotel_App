using HotelApp.Models;
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

    }
}
