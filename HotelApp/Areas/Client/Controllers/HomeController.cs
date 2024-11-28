using HotelApp.Areas.Client.Services;
using HotelApp.Areas.Client.ViewModels;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HotelApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class HomeController : Controller
    {
        

        [Route("Client")]
        [Route("Client/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Client/Rooms")]
        public IActionResult Rooms()
        {
            return View();
        }

        [Route("Client/TypePage")]
        public async Task<IActionResult> TypePage()
        {
            return View();
        }

        [Route("Client/VoucherPage")]
        public async Task<IActionResult> VoucherPage()
        {
            return View();
        }

        [Route("Client/ContactPage")]
        public async Task<IActionResult> ContactPage()
        {
            return View();
        }
        

    }
}
