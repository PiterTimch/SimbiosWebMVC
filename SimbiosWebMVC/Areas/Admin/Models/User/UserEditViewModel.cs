using System.ComponentModel.DataAnnotations;

namespace SimbiosWebMVC.Areas.Admin.Models.User
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new List<string>();
    }
}
