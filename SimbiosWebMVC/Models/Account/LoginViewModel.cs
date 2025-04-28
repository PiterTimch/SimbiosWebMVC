using System.ComponentModel.DataAnnotations;

namespace SimbiosWebMVC.Models.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Електронна пошта")]
        [Required(ErrorMessage = "Вкажіть електронну пошту")]
        [EmailAddress(ErrorMessage = "Пошту вказано неправильно")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
