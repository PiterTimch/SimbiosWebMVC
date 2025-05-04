using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimbiosWebMVC.Constants;
using SimbiosWebMVC.Data;
using SimbiosWebMVC.Data.Entities;
using SimbiosWebMVC.Interfaces;
using SimbiosWebMVC.Models.Category;

namespace SimbiosWebMVC.Controllers
{
    public class CategoriesController(AppDbContext context,
    IMapper mapper, IImageService imageService) : Controller
    {

        public IActionResult Index()
        {
            ViewBag.Title = "Категорії";
            var model = mapper.ProjectTo<CategoryItemViewModel>(context.Categories).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            var entity = await context.Categories.SingleOrDefaultAsync(x => x.Name == model.Name);
            if (entity != null)
            {
                ModelState.AddModelError("Name", "Така категорія уже є!!!");
                return View(model);
            }

            entity = mapper.Map<CategoryEntity>(model);
            entity.ImageUrl = await imageService.SaveImageAsync(model.ImageFile);
            await context.Categories.AddAsync(entity);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await context.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                await imageService.DeleteImageAsync(category.ImageUrl);
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.ImageName = category.ImageUrl;

            var model = mapper.Map<CategoryEditViewModel>(category);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (existing == null)
            {
                return NotFound();
            }

            var duplicate = await context.Categories
                .FirstOrDefaultAsync(x => x.Name == model.Name && x.Id != model.Id);
            if (duplicate != null)
            {
                ModelState.AddModelError("Name", "Another category with this name already exists");
                return View(model);
            }

            existing = mapper.Map(model, existing);

            if (model.ImageFile != null)
            {
                await imageService.DeleteImageAsync(existing.ImageUrl);
                existing.ImageUrl = await imageService.SaveImageAsync(model.ImageFile);
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
