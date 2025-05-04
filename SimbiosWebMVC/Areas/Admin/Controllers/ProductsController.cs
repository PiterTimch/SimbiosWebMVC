using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimbiosWebMVC.Areas.Admin.Models.Products;
using SimbiosWebMVC.Data;

namespace SimbiosWebMVC.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index(AppDbContext context,
        IMapper mapper)
        {
            ViewBag.Title = "Продукти";
            var model = mapper.ProjectTo<ProductItemViewModel>(context.Products).ToList();
            return View(model);
        }
    }
}
