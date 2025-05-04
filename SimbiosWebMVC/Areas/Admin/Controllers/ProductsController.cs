using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbiosWebMVC.Areas.Admin.Models.Product;
using SimbiosWebMVC.Constants;
using SimbiosWebMVC.Data;

namespace SimbiosWebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class ProductsController(AppDbContext context,
        IMapper mapper) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Продукти";
            var model = mapper.ProjectTo<ProductItemViewModel>(context.Products).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Створити продукт";
            ViewBag.Categories = context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewModel model)
        {
            ViewBag.Title = "Створити продукт";
            return View(model);
        }
    }
}
