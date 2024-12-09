using HotelApp.Data;
using HotelApp.Models;
using HotelApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace HotelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ClientController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("Manage/Client/Index")]
        public async Task<IActionResult> Index()
        {
            var clients = await _userManager.GetUsersInRoleAsync("Client");
            return View(clients.ToList());
        }

        [Route("Client/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Client/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.FullName = model.FirstName.Trim() + ' ' + model.LastName;
                user.UserName = model.Email.Trim();
                user.Email = model.Email.Trim();
                user.PhoneNumber = model.PhoneNumber.Trim();
                user.NormalizedEmail = model.Email.Trim().ToUpperInvariant();
                user.Password = model.Password.Trim();
                user.AvatarUrl = "~/upload/15122003basicavatar.png";
                var result = await _userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Client");
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }




        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _userManager.GetUsersInRoleAsync("Client");
            return Json(new { data = clients });
        }
        #endregion
    }
}
