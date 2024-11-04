using System.ComponentModel.DataAnnotations;

namespace HotelManage.Models
{
    public class Area
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
