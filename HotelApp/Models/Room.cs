using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManage.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
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

    }
}
