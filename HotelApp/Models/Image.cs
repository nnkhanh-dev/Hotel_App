using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HotelApp.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Path { get; set; }
        [Required]
        [StringLength(50)]
        public string Caption { get; set; }
        [ForeignKey(nameof(Room))]
        public int RoomID { get; set; }
        public Room Room { get; set; }
    }
}
