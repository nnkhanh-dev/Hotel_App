using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [EmailAddress]
        [Required]
        public string Email {  get; set; }
        [Required]
        public string NormalizedEmail { get; set; }
        [Required]
        public string PhoneNumber {  get; set; }
        [Required]
        public string Password { get; set; }
        public string? AvatarUrl { get; set; }

    }
}
