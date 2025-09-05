using HotelApp.Areas.Client.Services;
using HotelApp.Areas.Client.ViewModels;
using HotelApp.Data;
using HotelApp.Models;
using HotelApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class BookingController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IVNPayService _vnpayService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;

        public BookingController(ApplicationDbContext context, IVNPayService vnpayService, UserManager<AppUser> userManager, IMailService mailService)
        {
            _context = context;
            _vnpayService = vnpayService;
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpGet]
        [Route("Client/Bookings/")]
        public async Task<IActionResult> List()
        {
            var user = await _userManager.GetUserAsync(User);
            string userID = user.Id;

            var bookings = _context.Bookings
                            .Include(b => b.Room).ThenInclude(r => r.RoomType)
                            .Include(b => b.Room).ThenInclude(r => r.Area)
                            .Include(b => b.User)
                            .Include(b => b.Voucher)
                            .Where(b => b.UserID == userID && b.Status != -100)
                            .OrderByDescending(b => b.CreateAt) // Sắp xếp theo ngày tạo mới nhất
                            .Select(b => new BookingVM
                            {
                                Id = b.Id,
                                RoomId = b.RoomID,
                                UserId = user.Id,
                                CheckIn = b.CheckIn,
                                CheckOut = b.CheckOut,
                                CreateAt = b.CreateAt,
                                Total = b.Total,
                                Status = b.Status,
                                PayType = b.PayType,
                                TypeName = b.Room.RoomType.Name,
                                AreaName = b.Room.Area.Name
                            });

            return Json(new { Data = bookings });
        }


        [HttpPost]
        [Route("Client/Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (booking != null)
            {
                booking.Status = -1;
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đơn đặt phòng đã được hủy thành công." });
            }
            return Json(new { success = false, message = "Có lỗi xảy ra khi hủy đơn." });
        }



        [HttpGet]
        [Route("Client/BookingList/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Client/Booking/{id}/{checkin}/{checkout}")]
        public async Task<IActionResult> Booking(int id, DateTime checkin, DateTime checkout)
        {
            var user = await _userManager.GetUserAsync(User);
            string userID = user.Id;
            var information = await _context.Rooms
                               .Include(r => r.RoomType)
                               .Include(r => r.Area)
                               .Where(r => r.Id == id)
                               .Select(i => new BookingVM
                               {
                                   RoomId = i.Id,
                                   UserId = userID,
                                   CheckIn = checkin,
                                   CheckOut = checkout,
                                   TypeName = i.RoomType.Name,
                                   AreaName = i.Area.Name,
                                   CreateAt = DateTime.Now,
                                   Price = i.Price,
                                   DiscountRoom = i.Discount * 100,
                                   Total = (i.Price - (i.Discount * i.Price)) * (checkout - checkin).Days,
                                   Description = "Thanh toan " + i.RoomType.Name + " " + i.Area.Name
                               })
                               .FirstOrDefaultAsync();

            return View(information);
        }
        [HttpPost]
        [Route("Client/Booking/")]
        public async Task<IActionResult> Booking(BookingVM booking)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (booking.PayType == 1) // Thanh toán qua VNPay
                {
                    Booking b = new Booking
                    {
                        UserID = booking.UserId,
                        RoomID = booking.RoomId,
                        CheckIn = booking.CheckIn,
                        CheckOut = booking.CheckOut,
                        Status = -100,
                        Price = booking.Price,
                        Total = booking.Total,
                        PayType = booking.PayType,
                        CreateAt = booking.CreateAt,
                        Paid = booking.Total,
                        PaymentCode = booking.UserId.ToString() + booking.CreateAt.ToString("yyyyMMddHHmmss")
                    };

                    await _context.Bookings.AddAsync(b);
                    await _context.SaveChangesAsync();
                    return Redirect(_vnpayService.CreatePaymentUrl(HttpContext, booking));
                }
                else
                {
                    // Thanh toán khi nhận phòng
                    Booking b = new Booking
                    {
                        UserID = booking.UserId,
                        RoomID = booking.RoomId,
                        CheckIn = booking.CheckIn,
                        CheckOut = booking.CheckOut,
                        Status = 0,
                        Price = booking.Price,
                        Total = booking.Total,
                        PayType = booking.PayType,
                        CreateAt = booking.CreateAt
                    };

                    await _context.Bookings.AddAsync(b);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Đặt phòng thành công";
                    MailData mailData = new MailData();
                    mailData.ReceiverEmail = user.Email;
                    mailData.ReceiverName = user.FullName;
                    mailData.Title = "Thông báo đặt phòng";
                    mailData.Body = "Bạn đã đặt phòng thành công. Cảm ơn bạn đã đặt phòng của khách sạn chúng tôi, Hẹn gặp lại bạn nhé!";
                    _mailService.SendMail(mailData);
                    return RedirectToAction("Result");
                }
            }

            TempData["Message"] = "Đặt phòng thất bại";
            return RedirectToAction("Result");
        }
        [Route("Client/Result/")]

        public IActionResult Result()
        {
            return View();
        }

        [Route("Client/PaymentReturn/")]
        public async Task<IActionResult> PaymentReturn()
        {
            var user = await _userManager.GetUserAsync(User);
            var response = _vnpayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = "Thanh toán thất bại";
                return RedirectToAction("Result");
            }
            var code = response.OrderDescription;
            var booking = _context.Bookings.Where(b => b.PaymentCode == code).FirstOrDefault();
            booking.Status = 0;
            _context.SaveChanges();
            TempData["Message"] = "Đặt phòng thành công";
            MailData mailData = new MailData();
            mailData.ReceiverEmail = user.Email;
            mailData.ReceiverName = user.FullName;
            mailData.Title = "Thông báo đặt phòng";
            mailData.Body = "Bạn đã đặt phòng thành công. Cảm ơn bạn đã đặt phòng của khách sạn chúng tôi, Hẹn gặp lại bạn nhé!";
            _mailService.SendMail(mailData);
            return RedirectToAction("Result");
        }
    }
}
