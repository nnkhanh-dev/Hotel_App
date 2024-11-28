namespace HotelApp.Areas.Admin.ViewModels
{
    public class ChangePasswordVM
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
