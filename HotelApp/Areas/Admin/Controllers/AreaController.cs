using HotelApp.Data;
using HotelApp.Areas.Admin.ViewModels;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AreaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AreaController(ApplicationDbContext context) {
            _context = context;
        }

        [Route("Admin")]
        [Route("Area/Index")]
        public async Task<IActionResult> Index()
        {
            var areas = await _context.Areas.ToListAsync();
            return View(areas);
        }

        [Route("Area/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Area/Create")]
        public async Task<IActionResult> Create(AreaVM item)
        {
            Area area = new Area();

            area.Name = item.Name;

            if (ModelState.IsValid)
            {
                _context.Areas.Add(area);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        [HttpPost]
        [Route("Area/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound(); // Nếu không tìm thấy khu vực
            }

            _context.Areas.Remove(area); // Xóa khu vực
            await _context.SaveChangesAsync(); // Lưu thay đổi vào database

            return RedirectToAction("Index"); // Quay lại danh sách
        }
    }
}
