using SimbiosWebMVC.Models.Helpers;

namespace SimbiosWebMVC.Models.Product
{
    public class ProductListViewModel
    {
        public List<ProductItemViewModel> Products { get; set; } = new();
        public ProductSearchViewModel Search { get; set; } = new();
    }
}
