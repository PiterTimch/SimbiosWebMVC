using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimbiosWebMVC.Data;
using SimbiosWebMVC.Models.Helpers;
using SimbiosWebMVC.Models.Product;
using System.Threading.Tasks;

namespace SimbiosWebMVC.Controllers
{
    public class ProductsController(AppDbContext context, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(ProductSearchViewModel searchModel)
        {
            ViewBag.Title = "Продукти";

            searchModel.Categories = await mapper
                .ProjectTo<SelectItemViewModel>(context.Categories)
                .ToListAsync();

            searchModel.Categories.Insert(0, new SelectItemViewModel
            {
                Id = 0,
                Name = "Всі категорії"
            });

            var query = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                string textSearch = searchModel.Name.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(textSearch));
            }

            if (!string.IsNullOrEmpty(searchModel.Description))
            {
                string textSearch = searchModel.Description.Trim().ToLower();
                query = query.Where(p => p.Description.ToLower().Contains(textSearch));
            }

            if (searchModel.CategoryId != 0)
            {
                query = query.Where(p => p.CategoryId == searchModel.CategoryId);
            }

            int itemsPerPage = searchModel.Pagination.ItemsPerPage;
            int totalItems = await query.CountAsync();
            searchModel.Pagination.TotalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            searchModel.Pagination.TotalItems = totalItems;

            var products = await query
                .Skip((searchModel.Pagination.CurrentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ProjectTo<ProductItemViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            var model = new ProductListViewModel
            {
                Products = products,
                Search = searchModel,
            };

            return View(model);
        }
    }
}
