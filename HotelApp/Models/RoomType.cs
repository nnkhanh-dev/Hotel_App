using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        public int People { get; set; }
        [Required]
        [StringLength(500)]
        public string ImagePath { get; set; }

    }
}
