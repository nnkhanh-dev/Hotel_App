using HotelApp.Areas.Client.ViewModels;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("Account/Profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            ProfileVM profile = new ProfileVM
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = Url.Content(user.AvatarUrl)
            }; 
            return View(profile);
        }
        [HttpGet]
        [Route("Account/Edit")]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ProfileVM profile = new ProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl
            };
            return View(profile);
        }

        [HttpPost]
        [Route("Account/Edit")]
        public async Task<IActionResult> Edit(ProfileVM model, IFormFile Avatar = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Cập nhật thông tin người dùng
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.FullName = model.FirstName + " " + model.LastName;
                user.PhoneNumber = model.PhoneNumber;

                // Xử lý tải lên avatar
                if (Avatar != null && Avatar.Length > 0)
                {
                    // Tạo tên tệp mới với tiền tố là ticks
                    var fileExtension = Path.GetExtension(Avatar.FileName); // Lấy phần mở rộng của tệp
                    var fileName = DateTime.Now.Ticks.ToString()+ Avatar.FileName + fileExtension; // Dùng ticks làm tiền tố

                    // Lưu avatar vào thư mục trên server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Avatar.CopyToAsync(stream);
                    }

                    // Cập nhật URL của avatar
                    user.AvatarUrl = "~/upload/" + fileName;
                }

                // Cập nhật thông tin người dùng trong cơ sở dữ liệu
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("Profile");
                }
            }

            return View(model);
        }


        [HttpGet]
        [Route("Account/ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Route("Account/ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid) { 
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded) {
                user.Password = model.NewPassword;
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Profile");
            }
            return View(model);
        }
    }
}
