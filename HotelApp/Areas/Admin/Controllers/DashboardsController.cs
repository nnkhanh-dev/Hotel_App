using HotelApp.Areas.Admin.ViewModels;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HotelApp.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DashboardsController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    [Route("Dashboards/Index")]
    public async Task<IActionResult> Index()
    {
        var roomTypes = await _context.RoomTypes.ToListAsync();

        ViewBag.RoomTypes = roomTypes.Count();

        var areas = await _context.Areas.ToListAsync();

        ViewBag.Areas = areas.Count();

        var rooms = await _context.Rooms.ToListAsync();

        ViewBag.Rooms = rooms.Count();

        var clients = await _userManager.GetUsersInRoleAsync("Client");
        ViewBag.Clients = clients.Count;


        return View();
    }

    [Route("Dashboards/Revenue")]
    public async Task<IActionResult> Revenue()
    {
        var revenueData = await _context.Bookings
        .Where(b => b.Status == 3) 
        .GroupBy(b => new { b.CreateAt.Year, b.CreateAt.Month })
        .Select(g => new RevenueViewModel
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            Revenue = g.Sum(x => x.Total)
        })
        .OrderByDescending(r => r.Year)
        .ThenByDescending(r => r.Month)
        .ToListAsync();

        return Json(revenueData);
    }
}

