using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Interfaces;
using ePizzaHub.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace ePizzaHub.UI.Controllers
{
    public class AccountController : BaseController
    {
        private IAuthService _authService;
        private IQueueService _queueService;
        public AccountController(IAuthService authService, IQueueService queueService)
        {
            _authService = authService;
            _queueService = queueService;
        }
        public IActionResult Login()
        {
            return View();
        }
        async void GenerateTicket(UserModel user)
        {
            string strData = JsonSerializer.Serialize(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, strData),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, String.Join(",", user.Roles))
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
            });
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model, string returnUrl)
        {
            UserModel user = _authService.ValidateUser(model.Email, model.Password);
            if (user != null)
            {
                GenerateTicket(user);

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                if (user.Roles.Contains("User"))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "User" });
                }
                else if (user.Roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Invalid Credential's");
            }
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.Now
                };
                bool result = _authService.CreateUser(user, "User");
                if (result)
                {
                    var usr = new
                    {
                        UserName = user.Name,
                        Email = user.Email
                    };

                    //Adding to queue the user details to send email
                    _queueService.SendMessageAsync(usr, "userqueue");
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult UnAuthorize()
        {
            return View();
        }
    }
}
