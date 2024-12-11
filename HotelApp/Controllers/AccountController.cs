using HotelApp.Models;
using HotelApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using HotelApp.Areas.Client;
using Newtonsoft.Json;

namespace HotelApp.Controllers
{

    public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        [Route("/login")]
        [Route("/Account/Login")]

        public async Task<IActionResult> Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Dashboards", new { area = "Admin" });
                }
                else
                {
                    if (roles.Contains("Client"))
                    {
                        var bookingInfoJson = HttpContext.Session.GetString("BookingInfo");
                        if (!string.IsNullOrEmpty(bookingInfoJson))
                        {
                            // Deserialize thông tin từ Session
                            var bookingInfo = JsonConvert.DeserializeObject<dynamic>(bookingInfoJson);
                            int id = bookingInfo.Id;
                            string checkIn = bookingInfo.CheckIn;
                            string checkOut = bookingInfo.CheckOut;

                            // Xóa Session sau khi xử lý
                            HttpContext.Session.Remove("BookingInfo");

                            // Chuyển hướng đến trang Booking
                            return Redirect($"/Client/Booking/{id}/{checkIn}/{checkOut}");
                        }
                        return RedirectToAction("Index", "Home", new { area = "Client" });
                    }
                    else
                        return RedirectToAction("index", "Hotel");
                }
            }
            return View();
        }

        [HttpPost]
        [Route("/login")]
        [Route("/Account/Login")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var email = model.Email.Trim().ToUpperInvariant();
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {

                    //login
                    var result = await signInManager.PasswordSignInAsync(email!, model.Password!, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Index", "Dashboards", new { area = "Admin" });
                        }
                        else
                        {
                            if (roles.Contains("Client"))
                            {
                                var bookingInfoJson = HttpContext.Session.GetString("BookingInfo");
                                if (!string.IsNullOrEmpty(bookingInfoJson))
                                {
                                    // Deserialize thông tin từ Session
                                    var bookingInfo = JsonConvert.DeserializeObject<dynamic>(bookingInfoJson);
                                    int id = bookingInfo.Id;
                                    string checkIn = bookingInfo.CheckIn;
                                    string checkOut = bookingInfo.CheckOut;

                                    // Xóa Session sau khi xử lý
                                    HttpContext.Session.Remove("BookingInfo");

                                    // Chuyển hướng đến trang Booking
                                    return Redirect($"/Client/Booking/{id}/{checkIn}/{checkOut}");
                                }
                                return RedirectToAction("Index", "Home", new { area = "Client" });
                            }
                            else
                                return RedirectToAction("Index", "Hotel");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng nhập thất bại!");
                    }


                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                return View(model);
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {

                AppUser user = new AppUser();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.FullName = model.LastName.Trim() + ' ' + model.FirstName;
                user.UserName = model.Email.Trim();
                user.Email = model.Email.Trim();
                user.PhoneNumber = model.PhoneNumber.Trim();
                user.NormalizedEmail = model.Email.Trim().ToUpperInvariant();
                user.Password = model.Password.Trim();
                user.AvatarUrl = "~/upload/15122003basicavatar.png";
                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, "Client");
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model); // Nếu lỗi, hiển thị lại form đăng ký
                    }
                    await signInManager.SignInAsync(user, false);
                    var bookingInfoJson = HttpContext.Session.GetString("BookingInfo");
                    if (!string.IsNullOrEmpty(bookingInfoJson))
                    {
                        // Deserialize thông tin từ Session
                        var bookingInfo = JsonConvert.DeserializeObject<dynamic>(bookingInfoJson);
                        int id = bookingInfo.Id;
                        string checkIn = bookingInfo.CheckIn;
                        string checkOut = bookingInfo.CheckOut;

                        // Xóa Session sau khi xử lý
                        HttpContext.Session.Remove("BookingInfo");

                        // Chuyển hướng đến trang Booking
                        return Redirect($"/Client/Booking/{id}/{checkIn}/{checkOut}");
                    }
                    return RedirectToAction("Index", "Home", new { area = "Client" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [Route("/dang-xuat")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Hotel");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("opps")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}