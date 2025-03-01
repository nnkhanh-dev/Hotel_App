using HotelApp.Areas.Client.ViewModels;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/Booking/Index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/Booking/SelectClient/{userId}")]
        public async Task<IActionResult> SelectClient(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new BookingVM
            {
                UserId = userId,
                UserFullName = user.FullName,
                UserEmail = user.Email
            };

            return View("BookRoom", model);
        }

        [HttpGet]
        [Route("Admin/Booking/{id}/{checkin}/{checkout}")]
        public async Task<IActionResult> Booking(int id, DateTime checkin, DateTime checkout, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var information = await _context.Rooms
                               .Include(r => r.RoomType)
                               .Include(r => r.Area)
                               .Where(r => r.Id == id)
                               .Select(i => new BookingVM
                               {
                                   RoomId = i.Id,
                                   UserId = userId,
                                   UserFullName = user.FullName,
                                   UserEmail = user.Email,
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
        [Route("Admin/Booking/")]
        public async Task<IActionResult> Booking(BookingVM booking)
        {
            if (ModelState.IsValid)
            {
                Booking b = new Booking
                {
                    UserID = booking.UserId,
                    RoomID = booking.RoomId,
                    CheckIn = booking.CheckIn,
                    CheckOut = booking.CheckOut,
                    Status = 0,
                    Price = booking.Price,
                    Total = booking.Total,
                    PayType = 0,
                    CreateAt = booking.CreateAt
                };

                await _context.Bookings.AddAsync(b);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Đặt phòng thành công";
                return RedirectToAction("Result");
            }

            TempData["Message"] = "Đặt phòng thất bại";
            return RedirectToAction("Result");
        }

        [Route("Admin/Result/")]
        public IActionResult Result()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/Booking/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.CCCDs)
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r.RoomType)
                .Include(b => b.Room)
                .ThenInclude(r => r.Area)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            var viewModel = new BookingVM
            {
                Id = booking.Id,
                UserId = booking.UserID,
                UserFullName = booking.User.FullName,
                UserEmail = booking.User.Email,
                UserPhoneNumber = booking.User.PhoneNumber,
                RoomCode = booking.Room.Code,
                RoomId = booking.RoomID,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Price = booking.Price,
                Total = booking.Total,
                CreateAt = booking.CreateAt,
                Status = booking.Status,
                TypeName = booking.Room.RoomType.Name,
                AreaName = booking.Room.Area.Name,
                CCCD = booking.CCCDs.ToList()
            };

            return View(viewModel);
        }




        [HttpPost]
        [Route("Admin/Booking/Edit/{id}")]
        public async Task<IActionResult> Edit(BookingVM viewModel, IFormFileCollection newCCCDImages)
        {
            var booking = await _context.Bookings
                .Include(b => b.CCCDs)
                .FirstOrDefaultAsync(b => b.Id == viewModel.Id);

            if (booking == null) return NotFound();

            booking.Status = viewModel.Status;

            // Add new CCCD images
            if (newCCCDImages?.Count > 0)
            {
                for (int i = 0; i < newCCCDImages.Count; i += 2)
                {
                    var frontImg = newCCCDImages[i];
                    var backImg = newCCCDImages[i + 1];

                    var frontImgPath = Path.Combine("wwwroot/images/cccd", frontImg.FileName);
                    var backImgPath = Path.Combine("wwwroot/images/cccd", backImg.FileName);

                    using (var stream = new FileStream(frontImgPath, FileMode.Create))
                    {
                        await frontImg.CopyToAsync(stream);
                    }

                    using (var stream = new FileStream(backImgPath, FileMode.Create))
                    {
                        await backImg.CopyToAsync(stream);
                    }

                    booking.CCCDs.Add(new CCCD { FrontImg = "/images/cccd/" + frontImg.FileName, BackImg = "/images/cccd/" + backImg.FileName });
                }
            }

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Admin/Booking/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.CCCDs)
                .Include(b => b.User)
                .Include(b => b.Room)
                .ThenInclude(r => r.RoomType)
                .Include(b => b.Room)
                .ThenInclude(r => r.Area)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            var viewModel = new BookingVM
            {
                Id = booking.Id,
                UserId = booking.UserID,
                UserFullName = booking.User.FullName,
                UserEmail = booking.User.Email,
                UserPhoneNumber = booking.User.PhoneNumber,
                RoomCode = booking.Room.Code,
                RoomId = booking.RoomID,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Price = booking.Price,
                Total = booking.Total,
                CreateAt = booking.CreateAt,
                Status = booking.Status,
                TypeName = booking.Room.RoomType.Name,
                AreaName = booking.Room.Area.Name,
                CCCD = booking.CCCDs.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("Admin/Booking/DeleteImage")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var cccd = await _context.CCCDs.FindAsync(id);
            if (cccd == null)
            {
                return Json(new { success = false, message = "Image not found" });
            }

            _context.CCCDs.Remove(cccd);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Image deleted successfully" });
        }




        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> BookingList()
        {
            var bookings = await _context.Bookings
                            .Include(b => b.Room).ThenInclude(r => r.RoomType)
                            .Include(b => b.Room).ThenInclude(r => r.Area)
                            .Include(b => b.User)
                            .Include(b => b.Voucher)
                            .Where(b => b.Status != -100 && b.Status != -1)
                            .OrderByDescending(b => b.CreateAt)
                            .Select(b => new BookingVM
                            {
                                Id = b.Id,
                                RoomId = b.RoomID,
                                UserId = b.UserID,
                                CheckIn = b.CheckIn,
                                CheckOut = b.CheckOut,
                                CreateAt = b.CreateAt,
                                Total = b.Total,
                                Status = b.Status,
                                PayType = b.PayType,
                                TypeName = b.Room.RoomType.Name,
                                AreaName = b.Room.Area.Name,
                                RoomCode = b.Room.Code,
                                UserFullName = b.User.FullName,
                                UserEmail = b.User.Email
                            }).ToListAsync();

            return Json(new { Data = bookings });
        }
        #endregion
    }
}
