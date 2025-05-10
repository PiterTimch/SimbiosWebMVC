using SimbiosWebMVC.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SimbiosWebMVC.Models.Product
{
    public class ProductSearchViewModel
    {
        [Display(Name="Назва")]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Опис")]
        public string Description { get; set; } = String.Empty;

        [Display(Name="Категорія")]
        public int CategoryId { get; set; }

        public List<SelectItemViewModel> Categories { get; set; } = new();
    }
}
