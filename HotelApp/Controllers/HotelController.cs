using HotelApp.Models;
using HotelApp.Data;
using Microsoft.AspNetCore.Mvc;
using HotelApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace HotelApp.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HotelController(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Route("/Rooms")]
        public async Task<IActionResult> Rooms()
        {
            return View();
        }

        [Route("/TypePage")]
        public async Task<IActionResult> TypePage()
        {
            return View();
        }

        [Route("/VoucherPage")]
        public async Task<IActionResult> VoucherPage()
        {
            return View();
        }

        [Route("/ContactPage")]
        public async Task<IActionResult> ContactPage()
        {
            return View();
        }


        [Route("/LoaiPhong")]
        public IActionResult RoomType()
        {
            var types = _context.RoomTypes.Select(room => new
            {
                room.Id,
                room.Name,
                room.Description,
                room.People,
                ImagePath = Url.Content(room.ImagePath) // Chuyển "~" thành đường dẫn đầy đủ
            }).ToList();

            return Json(new {Data = types });

        }

        [Route("/UuDai")]
        public IActionResult Voucher()
        {
            var vouchers = _context.Vouchers
                .Where(v => v.Status == 0) // Lọc theo trạng thái
                .Select(v => new
                {
                    v.Id,
                    v.Name,
                    v.Description,
                    v.Quantity,
                    v.Code,
                    v.Status,
                    v.Discount,
                    ImagePath = Url.Content(v.ImagePath) // Chuyển "~" thành đường dẫn đầy đủ
                })
                .ToList();

            return Json(new { Data = vouchers });
        }

        [Route("/TienNghi")]
        public IActionResult TienNghi()
        {
            var amenities = _context.Amenities.ToList();
            return Json(new { Data = amenities });
        }

        [Route("/KhuVuc")]
        public IActionResult KhuVuc()
        {
            var areas = _context.Areas.ToList();
            return Json(new { Data = areas });
        }
        [Route("/TrangThai")]
        public IActionResult TrangThai()
        {
            var areas = _context.Areas.ToList();
            return Json(new { Data = areas });
        }
        [HttpPost]
        [Route("/Hotel/Contact")]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Contacts.Add(contact);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Gửi liên hệ thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Gửi liên hệ thất bại. Vui lòng thử lại.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại.";
            }
            return View(contact);
        }
        [Route("/List/{checkIn?}/{checkOut?}")]
        public async Task<IActionResult> List(DateTime? checkIn, DateTime? checkOut)
        {
            // Lấy danh sách RoomID của các phòng đang bận
            var busyRoomIDs = _context.Bookings
                .Where(b => b.Status != 3 && b.Status != -100 &&// Loại trừ trạng thái phòng "trống"
                            (checkIn == null || checkOut == null || // Nếu không có ngày, bỏ qua điều kiện thời gian
                            (b.CheckIn <= checkOut && b.CheckOut >= checkIn))) // Điều kiện trùng thời gian
                .Select(b => b.RoomID)
                .Distinct()
                .ToList();

            // Lọc các phòng theo các tham số (nếu có)
            var roomQuery = _context.Rooms 
                .Include(r => r.RoomType)
                .Include(r => r.Area)
                .Include(r => r.Amenities)
                .Include(r => r.Images)
                .Where(r => !busyRoomIDs.Contains(r.Id));

            // Lấy danh sách phòng
            var roomVMs = await roomQuery
                .Select(r => new RoomVM
                {
                    Id = r.Id,
                    TypeId = r.TypeId,
                    TypeName = r.RoomType.Name,
                    AreaId = r.AreaId,
                    AreaName = r.Area.Name,
                    Price = r.Price,
                    Discount = r.Discount,
                    Status = r.Status,
                    StatusStr = r.Status == 0 ? "Sẵn sàng" : r.Status == 1 ? "Đang sử dụng" : "Bảo trì",
                    ImageUrls = r.Images.Select(img => Url.Content(img.Path)).ToList(),
                    AmenityNames = r.Amenities.Select(a => a.Name).ToList()
                })
                .ToListAsync();

            return Json(new
            {
                data = roomVMs
            }); // Trả dữ liệu dưới dạng JSON
        }

        [Route("/RoomDetail/{id}")]
        public async Task<IActionResult> RoomDetail(int id)
        {
            var roomVM = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.Area)
                .Include(r => r.Amenities)
                .Include(r => r.Images)
                .Where(r => r.Id == id)
                .Select(r => new RoomVM
                {
                    Id = r.Id,
                    TypeId = r.TypeId,
                    TypeName = r.RoomType.Name,
                    AreaId = r.AreaId,
                    AreaName = r.Area.Name,
                    Price = r.Price,
                    Discount = r.Discount ,
                    Status = r.Status,
                    StatusStr = r.Status == 0 ? "Đang hoạt động" : r.Status == 1 ? "Bảo trì" : "Dừng hoạt động",
                    ImageUrls = r.Images.Select(img => Url.Content(img.Path)).ToList(),
                    AmenityNames = r.Amenities.Select(a => a.Name).ToList(),
                    People = r.RoomType.People,
                    Description = r.RoomType.Description,
                    Total = r.Price - r.Discount * r.Price
                }).FirstOrDefaultAsync();
            if(roomVM == null)
            {
                return NotFound("Không tìm thấy phòng!");
            }

            return PartialView("_RoomDetail", roomVM);
        }
        [Route("/Booking/{id}/{checkin}/{checkout}")]
        public async Task<IActionResult> Booking(int id, DateTime checkin, DateTime checkout)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Client"))
                {
                    // Định dạng lại checkin và checkout thành chuỗi ngày theo định dạng mong muốn
                    string formattedCheckIn = checkin.ToString("yyyy-MM-dd");
                    string formattedCheckOut = checkout.ToString("yyyy-MM-dd");

                    // Tạo URL với các tham số đã định dạng
                    string url = $"/Client/Booking/{id}/{formattedCheckIn}/{formattedCheckOut}";
                    return Redirect(url);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                HttpContext.Session.SetString("BookingInfo", JsonConvert.SerializeObject(new
                {
                    Id = id,
                    CheckIn = checkin.ToString("yyyy-MM-dd"),
                    CheckOut = checkout.ToString("yyyy-MM-dd")
                }));
                return RedirectToAction("Login", "Account");
            }
        }

        [Route("/GetListTopRoom")]
        public async Task<IActionResult> GetListTopRoom()
        {
            var topRooms = await _context.Bookings
                .Where(b => b.Status != -100) // Loại trừ các booking bị hủy
                .GroupBy(b => b.RoomID)
                .Select(g => new
                {
                    RoomID = g.Key,
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(20)
                .Join(_context.Rooms.Include(r => r.RoomType).Include(r => r.Area).Include(r => r.Images).Include(r => r.Amenities),
                    booking => booking.RoomID,
                    room => room.Id,
                    (booking, room) => new
                    {
                        Id = room.Id,
                        TypeName = room.RoomType.Name,
                        AreaName = room.Area.Name,
                        Price = room.Price,
                        Discount = room.Discount,
                        Status = room.Status,
                        StatusStr = room.Status == 0 ? "Sẵn sàng" : room.Status == 1 ? "Đang sử dụng" : "Bảo trì",
                        ImageUrls = room.Images.Select(img => Url.Content(img.Path)).ToList(),
                        AmenityNames = room.Amenities.Select(a => a.Name).ToList(),
                        BookingCount = booking.BookingCount
                    })
                .ToListAsync();

            return Json(new { data = topRooms });
        }

        [Route("/CheckRoomAvailability/{id}")]
        public async Task<IActionResult> CheckRoomAvailability(int id, DateTime checkIn, DateTime checkOut)
        {
            // Kiểm tra ngày hợp lệ
            if (checkIn >= checkOut)
            {
                return Json(new { success = false, message = "Ngày đến phải trước ngày đi!" });
            }

            if (checkIn < DateTime.Today)
            {
                return Json(new { success = false, message = "Ngày đến không được là ngày trong quá khứ!" });
            }

            // Kiểm tra phòng có tồn tại không
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return Json(new { success = false, message = "Phòng không tồn tại!" });
            }

            // Kiểm tra phòng có bị đặt trong khoảng thời gian này không
            var isBooked = await _context.Bookings
                .AnyAsync(b => b.RoomID == id && 
                              b.Status != 3 && b.Status != -100 && // Loại trừ đã trả phòng và đã hủy
                              b.CheckIn < checkOut && b.CheckOut > checkIn);

            if (isBooked)
            {
                return Json(new { success = false, message = "Phòng đã được đặt trong khoảng thời gian này. Vui lòng chọn ngày khác!" });
            }

            return Json(new { success = true, message = "Phòng còn trống!" });
        }

        
    }
}
