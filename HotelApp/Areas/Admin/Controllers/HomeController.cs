using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        [Route("Admin")]
        [Route("Admin/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
