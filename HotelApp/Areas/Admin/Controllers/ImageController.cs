
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ImageController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Admin")]
        [Route("Image/Index")]
        public async Task<IActionResult> Index()
        {
            var areas = await _context.Images.ToListAsync();
            return View(areas);
        }  
    }
}