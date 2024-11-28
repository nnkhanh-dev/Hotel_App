using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Route("RoomType/Index")]
        public async Task<IActionResult> Index()
        {
            var areas = await _context.RoomTypes.ToListAsync();
            return View(areas);
        }
        [Route("RoomType/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("RoomType/Create")]
        public async Task<IActionResult> Create(RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                _context.RoomTypes.Add(roomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(roomType);
        }


    }
}
