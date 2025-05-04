using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimbiosWebMVC.Data;
using SimbiosWebMVC.Models.Product;

namespace SimbiosWebMVC.Controllers
{
    public class ProductsController(AppDbContext context,
    IMapper mapper) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Продукти";
            var model = mapper.ProjectTo<ProductItemViewModel>(context.Products).ToList();
            return View(model);
        }
    }
}
