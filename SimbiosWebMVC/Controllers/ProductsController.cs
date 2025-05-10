using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimbiosWebMVC.Data;
using SimbiosWebMVC.Models.Helpers;
using SimbiosWebMVC.Models.Product;
using System.Threading.Tasks;

namespace SimbiosWebMVC.Controllers
{
    public class ProductsController(AppDbContext context,
    IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Продукти";
            var searchModel = new ProductSearchViewModel();

            searchModel.Categories = await mapper
                .ProjectTo<SelectItemViewModel>(context.Categories)
                .ToListAsync();

            var model = new ProductListViewModel();

            searchModel.Categories.Insert(0, new SelectItemViewModel
            {
                Id = 0,
                Name = "Всі категорії"
            });

            model.Products = mapper.ProjectTo<ProductItemViewModel>(context.Products).ToList();
            model.Search = searchModel;

            return View(model);
        }
    }
}
