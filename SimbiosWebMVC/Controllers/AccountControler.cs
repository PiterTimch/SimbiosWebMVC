using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimbiosWebMVC.Data.Entities.Identity;
using SimbiosWebMVC.Interfaces;
using SimbiosWebMVC.Models.Account;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Threading.Tasks;
using SimbiosWebMVC.SMTP;

namespace SimbiosWebMVC.Controllers
{
    public class AccountController(UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager, IImageService imageService
        , IMapper mapper, ISMTPService smptService) : Controller
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
                    await signInManager.SignInAsync(user, false);

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

            var user = mapper.Map<UserEntity>(model);
            user.Image = await imageService.SaveImageAsync(model.Image) ?? null;

            var res = await userManager.CreateAsync(user, model.Password);
            if (res.Succeeded)
            {
                await signInManager.SignInAsync(user, false);

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
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Такого користувача не існує");
                return View(model);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resultUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

            Message msg = new Message
            {
                Body = $"Перейди і ресетни пароль <a href='{resultUrl}'>Скинути</a>",
                Subject = $"Скидання паролю",
                To = model.Email
            };

            var result = await smptService.SendEmailAsync(msg);

            if (!result)
            {
                ModelState.AddModelError("", "Помилка надсилання листа");
                return View(model);
            }

            return RedirectToAction(nameof(ForgotPasswordSend));
        }

        [HttpGet]
        public IActionResult ForgotPasswordSend()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email, string token) 
        {
            //var user = await userManager.FindByEmailAsync(email);
            //var result = await userManager.ResetPasswordAsync();

            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            return View();
        }
    }
}
