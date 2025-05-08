﻿using System.ComponentModel.DataAnnotations;

namespace SimbiosWebMVC.Models.Account
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Новий Пароль")]
        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Підтвердіть пароль")]
        [Required(ErrorMessage = "Напишіть пароль ще раз")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
