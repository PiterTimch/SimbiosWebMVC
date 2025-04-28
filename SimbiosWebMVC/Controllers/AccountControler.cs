using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimbiosWebMVC.Data.Entities.Identity;
using SimbiosWebMVC.Interfaces;
using SimbiosWebMVC.Models.Account;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SimbiosWebMVC.Controllers
{
    public class AccountController(UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager, IImageService imageService) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var res = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (res.Succeeded)
                {
                    var userClaims = await userManager.GetClaimsAsync(user);

                    if (string.IsNullOrEmpty(userClaims.FirstOrDefault(c => c.Type == "Image")?.Value))
                    {
                        userClaims.Add(new Claim("Image", user.Image ?? ""));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await signInManager.SignInWithClaimsAsync(user, false, claimsIdentity.Claims);

                    return Redirect("/");
                }
            }

            ModelState.AddModelError("", "Невірний логін або пароль.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = new UserEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                Image = await imageService.SaveImageAsync(model.Image) ?? null
            };
            var res = await userManager.CreateAsync(user, model.Password);
            if (res.Succeeded)
            {
                var userClaims = await userManager.GetClaimsAsync(user);

                if (string.IsNullOrEmpty(userClaims.FirstOrDefault(c => c.Type == "Image")?.Value))
                {
                    userClaims.Add(new Claim("Image", user.Image ?? ""));
                }

                var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                await signInManager.SignInWithClaimsAsync(user, false, claimsIdentity.Claims);

                return Redirect("/");
            }
            foreach (var error in res.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [Authorize]
        public IActionResult AccessDenied(string returnUrl)
        {
            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var user = userManager.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (user == null)
                return RedirectToAction("Login");

            var model = new ProfileViewModel()
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Image = user.Image ?? ""
            };

            return View(model);
        }
    }
}
