using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(255)]
        public string Message { get; set; }
    }
}
