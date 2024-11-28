using HotelApp.Areas.Admin.ViewModels;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AmenityController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public AmenityController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Admin")]
        [Route("Amenity/Index")]
        public async Task<IActionResult> Index()
        {
            var areas = await _context.Amenities.ToListAsync();
            return View(areas);
        }

        [Route("Amenity/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Amenity/Create")]
        public async Task<IActionResult> Create(AmenityVM item)
        {
            Amenity amenity = new Amenity();

            amenity.Name = item.Name;
            amenity.Description = item.Description;

            if (ModelState.IsValid)
            {
                _context.Amenities.Add(amenity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(amenity);
        }

        [HttpPost]
        [Route("Amenity/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound(); // Nếu không tìm thấy khu vực
            }

            _context.Amenities.Remove(amenity); // Xóa khu vực
            await _context.SaveChangesAsync(); // Lưu thay đổi vào database

            return RedirectToAction("Index"); // Quay lại danh sách
        }
    }
}