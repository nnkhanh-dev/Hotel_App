using HotelApp.Areas.Client.Services;
using HotelApp.Areas.Client.ViewModels;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HotelApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IVNPayService _vnpayService;
        public HomeController(ApplicationDbContext context, UserManager<AppUser> userManager, IVNPayService vnpayService)
        {
            _context = context;
            _userManager = userManager;
            _vnpayService = vnpayService;
        }


        [Route("Client")]
        [Route("Client/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Client/Rooms")]
        public IActionResult Rooms()
        {
            return View();
        }

        [Route("Client/TypePage")]
        public async Task<IActionResult> TypePage()
        {
            return View();
        }

        [Route("Client/VoucherPage")]
        public async Task<IActionResult> VoucherPage()
        {
            return View();
        }

        [Route("Client/ContactPage")]
        public async Task<IActionResult> ContactPage()
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
                               .Select(i => new BookingVM {
                                   RoomId = i.Id,
                                   UserId = userID,
                                   CheckIn = checkin,
                                   CheckOut = checkout,
                                   TypeName = i.RoomType.Name,
                                   AreaName = i.Area.Name,
                                   CreateAt = DateTime.Now,
                                   Price = i.Price,
                                   DiscountRoom = i.Discount*100,
                                   Total = (i.Price - (i.Discount*i.Price)) * (checkout - checkin).Days,
                                   Description = "Thanh toan " + i.RoomType.Name + " " +i.Area.Name
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
        public IActionResult PaymentReturn()
        {
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
            return RedirectToAction("Result");
        }

    }
}
