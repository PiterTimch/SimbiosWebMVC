using System.ComponentModel.DataAnnotations;

namespace SimbiosWebMVC.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "Ваш емейл")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [EmailAddress(ErrorMessage = "Некоректний формат емейлу")]
        public string Email { get; set; } = string.Empty;
    }
}
