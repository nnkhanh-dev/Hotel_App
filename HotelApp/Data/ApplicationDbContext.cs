using HotelApp.Models;
using HotelManage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HotelManage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
            public DbSet<RoomType> RoomTypes { get; set; }
            public DbSet<Amenity> Amenities { get; set; }
            public DbSet<Room> Rooms { get; set; }
            public DbSet<Voucher> Vouchers { get; set; }
            public DbSet<Image> Images { get; set; }
            public DbSet<Area> Areas { get; set; }
            public DbSet<Contact> Contacts { get; set; }

    }
}
