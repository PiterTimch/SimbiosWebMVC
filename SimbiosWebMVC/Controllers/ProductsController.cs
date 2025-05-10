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
        [HttpGet]
        public async Task<IActionResult> Index(ProductSearchViewModel searchModel)
        {
            ViewBag.Title = "Продукти";
            searchModel = new ProductSearchViewModel();

            searchModel.Categories = await mapper
                .ProjectTo<SelectItemViewModel>(context.Categories)
                .ToListAsync();

            var model = new ProductListViewModel();

            searchModel.Categories.Insert(0, new SelectItemViewModel
            {
                Id = 0,
                Name = "Всі категорії"
            });

            var query = context.Products.AsQueryable();

            model.Products = mapper.ProjectTo<ProductItemViewModel>(query).ToList();
            model.Search = searchModel;

            model.Count = query.Count();

            return View(model);
        }
    }
}
