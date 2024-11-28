using HotelApp.Data;
using HotelApp.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApp.Models;

namespace HotelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Admin")]
        [Route("Room/Index")]
        public async Task<IActionResult> Index()
        {
            var areas = await _context.Rooms.ToListAsync();
            return View(areas);
        }

        [Route("Room/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Room/Create")]
        public async Task<IActionResult> Create(RoomVM item)
        {
            Room room = new Room();
            room.AreaId = item.AreaId;
            room.TypeId = item.TypeId;
            room.Price = item.Price;
            room.Discount = item.Discount;
            room.Status = item.Status;

            if (ModelState.IsValid)
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }
    }
}
