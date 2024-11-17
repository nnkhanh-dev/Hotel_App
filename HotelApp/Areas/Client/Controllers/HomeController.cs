using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class HomeController : Controller
    {
        [Route("Client/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
