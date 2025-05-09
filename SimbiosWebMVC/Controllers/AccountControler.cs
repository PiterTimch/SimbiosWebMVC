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

            string emailBody = @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <style>
                    body { font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; }
                    .email-container { background-color: #ffffff; padding: 20px; border-radius: 8px; max-width: 600px; margin: auto; border: 1px solid #dddddd; }
                    .btn { background-color: #0d6efd; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 4px; display: inline-block; }
                    .btn:hover { background-color: #0b5ed7; }
                    .footer { margin-top: 20px; font-size: 12px; color: #666666; }
                </style>
            </head>
            <body>
                <table class='email-container'>
                    <tr>
                        <td style='text-align: center;'>
                            <h2>Скидання паролю</h2>
                            <p style='margin: 10px'>Ви отримали цей лист, тому що ми отримали запит на скидання паролю для вашого облікового запису.</p>
                            <a href='{resultUrl}' class='btn'>Скинути пароль</a>
                            <p style='margin: 10px'> Якщо ви не надсилали цей запит, просто ігноруйте цей лист.</p>
                            <p class='footer'>© 2025 Категорік. Усі права захищено.</p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            ";

            emailBody = emailBody.Replace("{resultUrl}", resultUrl);

            Message msg = new Message
            {
                Body = emailBody,
                Subject = "Скидання паролю",
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
            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("" ,"Користувача з такою поштою не існує");
                return View(model);
            }

            bool isCurrentPassword = await userManager.CheckPasswordAsync(user, model.Password);
            if (isCurrentPassword)
            {
                ModelState.AddModelError("", "Новий пароль не може бути таким же, як поточний пароль.");
                return View(model);
            }

            var result = userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Result.Succeeded)
            {
                ModelState.AddModelError("", "Помилка скидання паролю");
                return View(model);
            }

            Message succeedMessage = new Message
            {
                Body = $"Пароль успішно скинуто",
                Subject = $"Скидання паролю",
                To = model.Email
            };

            return RedirectToAction(nameof(SucceedResetPassword));
        }

        [HttpGet]
        public IActionResult SucceedResetPassword() 
        {
            return View();
        }
    }
}
