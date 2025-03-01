using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }

        [EmailAddress]
        [Required]
        public override string Email { get; set; }

        public override string? NormalizedEmail { get; set; }

        [Required]
        public override string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
