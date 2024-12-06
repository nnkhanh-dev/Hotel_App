using System.ComponentModel.DataAnnotations;

namespace HotelApp.Areas.Admin.ViewModels
{
    public class ProfileVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
