using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelApp.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardsController : Controller
{
    [Route("Dashboards/Index")]
    public IActionResult Index() => View();
}
