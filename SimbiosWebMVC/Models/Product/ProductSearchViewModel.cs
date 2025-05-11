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

        [Display(Name="Елементів на сторінці")]
        public int ItemsPerPage { get; set; } = 5;

        public List<int> Elements { get; set; } = new() { 5, 10, 15, 20 };
    }
}
