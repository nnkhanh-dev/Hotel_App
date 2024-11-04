using HotelApp.Models;
using HotelManage.Data;
using Microsoft.AspNetCore.Mvc;

namespace HotelManage.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
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

    }
}
