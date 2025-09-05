using HotelApp.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HotelApp.Areas.Client.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        //injection MailSetting vào lớp này để dùng
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }
        // xử lý gửi mail
        public bool SendMail(MailData mailData)
        {
            using (MimeMessage emailMessage = new MimeMessage())
            {
                var emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                var emailTo = new MailboxAddress(mailData.ReceiverName, mailData.ReceiverEmail);

                emailMessage.From.Add(emailFrom);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = mailData.Title;

                var emailBodyBuilder = new BodyBuilder();

                // Gán nội dung HTML vào HtmlBody
                emailBodyBuilder.HtmlBody = @"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Email</title>
                        <style>
                            .container
                            {
                                        width: 100%;
                                        padding: 64px 0;
                                        background-image: url('https://images.pexels.com/photos/258154/pexels-photo-258154.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2');
                                        background-position: center;
                                        background-size: cover;
                                        background-repeat: no-repeat;
                                        display: flex;
                                        justify-content: center;
                                        align-items: center;
                             }
                            .card {
                                border: 1px solid #ccc;
                                width: 500px;
                                margin: auto;
                                font-family: Arial, sans-serif;
                            }
                            .card-header {
                                padding: 16px 32px;
                                text-align: center;
                                background-color: black;
                                color: white;
                                font-size: 20px;
                                font-weight: bold;
                            }
                            .card-body {
                                padding: 32px 32px;
                                text-align: center;
                                font-size: 18px;
                                background-color: white;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='card'>
                                <div class='card-header'>
                                    THÔNG BÁO ĐẶT PHÒNG
                                </div>
                                <div class='card-body'>"
                                    + mailData.Body +
                                @"</div>
                            </div>
                        </div>
                    </body>
                    </html>";

                // Không dùng TextBody nếu đã dùng HtmlBody, tránh email xuống dòng không đẹp
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using (var mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }

            return true;
        }

    }
}
