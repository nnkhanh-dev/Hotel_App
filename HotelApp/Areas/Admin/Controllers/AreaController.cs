using HotelApp.Data;
using HotelApp.Areas.Admin.ViewModels;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApp.Areas.Client.ViewModels;

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

        [HttpGet]
        [Route("Area/ListArea")]
        public async Task<IActionResult> ListArea()
        {
            var areas = await _context.Areas.ToListAsync();
            return Json(new { Data = areas });
        }

        [Route("Manage/Area/Index")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("Area/Create")]
        public IActionResult Create()
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

        [HttpGet]
        [Route("Area/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }
        [HttpGet]
        [Route("Area/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Area/Edit/{id}")]
        public async Task<IActionResult> Edit(Area model)
        {
            if (ModelState.IsValid)
            {
                _context.Areas.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Area/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound(); // Nếu khu vực không tồn tại
            }

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();

            return Json(new { success = true }); // Trả về JSON thành công cho AJAX
        }


    }
}
