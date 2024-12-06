using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelApp.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PagesController : Controller
{
    [HttpGet]
    [Route("Pages/AccountSettings")]
    public IActionResult AccountSettings() => View();
    [HttpGet]
    [Route("Pages/AccountSettingsConnections")]
    public IActionResult AccountSettingsConnections() => View();
    [HttpGet]
    [Route("Pages/AccountSettingsNotifications")]
    public IActionResult AccountSettingsNotifications() => View();

    [HttpGet]
    [Route("Pages/MiscError")]
    public IActionResult MiscError() => View();

    [HttpGet]
    [Route("Pages/MiscUnderMaintenance")]
    public IActionResult MiscUnderMaintenance() => View();
}
