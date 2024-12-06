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
    public class RoomTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Admin")]
        [Route("Manage/RoomType/Index")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("RoomType/ListRoomType")]
        public async Task<IActionResult> ListRoomType()
        {
            var roomTypes = await _context.RoomTypes.ToListAsync();
            foreach (var a in roomTypes)
            {
                a.ImagePath = Url.Content(a.ImagePath);
            }
            return Json(new { Data = roomTypes });
        }

        [Route("RoomType/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("RoomType/Create")]
        public async Task<IActionResult> Create(RoomType roomType, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
            {
                // Tạo tên tệp mới với thời gian hiện tại để tránh trùng lặp
                var fileExtension = Path.GetExtension(Image.FileName); // Lấy phần mở rộng
                var fileName = $"{DateTime.Now.Ticks}{fileExtension}"; // Tên file với ticks

                // Đường dẫn lưu file trong wwwroot/upload
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                // Lưu đường dẫn file vào cơ sở dữ liệu
                roomType.ImagePath = "~/upload/" + fileName;
            }
                _context.RoomTypes.Add(roomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(roomType);
        }



        [HttpGet]
        [Route("RoomType/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            roomType.ImagePath = Url.Content(roomType.ImagePath);
            return View(roomType); // Trả về View hiển thị chi tiết
        }
        [HttpGet]
        [Route("RoomType/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }
            roomType.ImagePath = Url.Content(roomType.ImagePath);

            return View(roomType); // Trả về View với form chỉnh sửa
        }
        [HttpPost]
        [Route("RoomType/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomType roomType)
        {
            if (id != roomType.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    roomType.ImagePath = '~' + roomType.ImagePath;
                    _context.Update(roomType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.RoomTypes.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index)); // Quay lại danh sách sau khi cập nhật
            }

            return View(roomType); // Trả lại form nếu model không hợp lệ
        }
        [HttpPost]
        [Route("RoomType/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync();

            return Json(new { success = true }); // Trả về JSON cho AJAX
        }
    }
}
