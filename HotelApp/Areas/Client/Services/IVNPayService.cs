using HotelApp.Areas.Client.ViewModels;
using HotelApp.ViewModels;

namespace HotelApp.Areas.Client.Services
{
    public interface IVNPayService
    {
        // Tạo URL thanh toán VNPay
        string CreatePaymentUrl(HttpContext context, BookingVM booking);

        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
