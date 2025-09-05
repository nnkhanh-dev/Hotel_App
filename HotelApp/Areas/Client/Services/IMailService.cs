using HotelApp.ViewModels;

namespace HotelApp.Areas.Client.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
