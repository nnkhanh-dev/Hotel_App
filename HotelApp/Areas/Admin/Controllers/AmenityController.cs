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

        [Route("Manage/Amenity/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Amenity/ListAmenity")]
        public async Task<IActionResult> ListAmenity()
        {
            var amenities = await _context.Amenities.ToListAsync();
            return Json(new { Data = amenities });
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
        [HttpGet]
        [Route("Amenity/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound();
            }
            return View(amenity);
        }
        [HttpGet]
        [Route("Amenity/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound();
            }
            return View(amenity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Amenity/Edit/{id}")]
        public async Task<IActionResult> Edit(Amenity model)
        {
            if (ModelState.IsValid)
            {
                _context.Amenities.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Amenity/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound(); // Nếu khu vực không tồn tại
            }

            _context.Amenities.Remove(amenity);
            await _context.SaveChangesAsync();

            return Json(new { success = true }); // Trả về JSON thành công cho AJAX
        }


    }
}
