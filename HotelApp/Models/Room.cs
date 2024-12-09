using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        [ForeignKey(nameof(RoomType))]
        public int TypeId { get; set; }
        [ForeignKey(nameof(Area))]
        public int AreaId { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        public int Status { get; set; }
        public RoomType RoomType { get; set; }
        public Area Area { get; set; }

        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
        public ICollection<Image> Images { get; set; } = new List<Image>();

    }
}
