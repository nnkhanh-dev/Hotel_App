using System.ComponentModel.DataAnnotations;

namespace HotelApp.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        public string? NormalizedEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmed password is required.")]
        [Compare("Password", ErrorMessage = "The confirmed password does not match the password.")]
        public string ConfirmedPassword { get; set; }
    }
}
