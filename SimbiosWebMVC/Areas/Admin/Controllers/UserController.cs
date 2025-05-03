using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimbiosWebMVC.Areas.Admin.Models.User;
using SimbiosWebMVC.Data.Entities.Identity;
using SimbiosWebMVC.Interfaces;
using SimbiosWebMVC.Services;
using System.Threading.Tasks;

namespace SimbiosWebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController(IMapper mapper, UserManager<UserEntity> userManager, IImageService imageService) : Controller
    {
        public async Task<IActionResult> Index()
        {

            var model = await userManager.Users
                .ProjectTo<UserItemViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            //криво косо, але працює :)
            model.ForEach(user =>
            {
                user.Roles = userManager.GetRolesAsync((userManager
                    .FindByEmailAsync(user.Email).Result)).Result
                    .ToList();
            });

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var model = mapper.Map<UserEditViewModel>(user);

            model.Roles = (await userManager.GetRolesAsync(user)).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();

            var currentRoles = await userManager.GetRolesAsync(user);

            mapper.Map(model, user);

            if (model.ImageFile != null &&
                !string.IsNullOrEmpty(model.ImageFile.FileName) &&
                !model.ImageFile.FileName.Equals(model.ImageName, StringComparison.OrdinalIgnoreCase))
            {
                user.Image = await imageService.SaveImageAsync(model.ImageFile);
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var rolesToAdd = model.Roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(model.Roles).ToList();

            if (rolesToRemove.Any())
                await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (rolesToAdd.Any())
                await userManager.AddToRolesAsync(user, rolesToAdd);

            return RedirectToAction("Index");
        }
    }
}
