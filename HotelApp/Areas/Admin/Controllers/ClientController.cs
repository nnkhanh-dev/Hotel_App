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
        public async Task<IActionResult> Create(RegisterVM model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = model.FirstName.Trim() + ' ' + model.LastName,
                    UserName = model.Email.Trim(),
                    Email = model.Email.Trim(),
                    PhoneNumber = model.PhoneNumber.Trim(),
                    NormalizedEmail = model.Email.Trim().ToUpperInvariant(),
                    Password = model.Password.Trim(),
                    AvatarUrl = "~/upload/15122003basicavatar.png"
                };
                var result = await _userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Client");
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [Route("Client/Select")]
        [HttpGet]
        public async Task<IActionResult> SelectClient()
        {
            var clients = await _userManager.GetUsersInRoleAsync("Client");
            return View(clients.ToList());
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
