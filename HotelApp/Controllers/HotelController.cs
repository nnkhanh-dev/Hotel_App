using HotelApp.Models;
using HotelApp.Data;
using Microsoft.AspNetCore.Mvc;
using HotelApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
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
                .Where(b => b.Status != 3 && // Loại trừ trạng thái phòng "trống"
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
                    AmenityNames = r.Amenities.Select(a => a.Name).ToList()
                }).FirstOrDefaultAsync();
            if(roomVM == null)
            {
                return NotFound("Không tìm thấy phòng!");
            }

            return PartialView("_RoomDetail", roomVM);
        }
    
    }
}
