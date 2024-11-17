using System.ComponentModel.DataAnnotations;

namespace HotelApp.ViewModels
{
    public class LoginVM
    {
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ tài khoản")]
        public bool RememberMe { get; set; }
    }
}
