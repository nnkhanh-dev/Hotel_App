using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();

    }
}
