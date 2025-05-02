using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimbiosWebMVC.Areas.Admin.Models.User;
using SimbiosWebMVC.Data.Entities.Identity;
using System.Threading.Tasks;

namespace SimbiosWebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController(IMapper mapper, UserManager<UserEntity> userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {

            var model = await userManager.Users
                .ProjectTo<UserItemViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            model.ForEach(user =>
            {
                user.Roles = userManager.GetRolesAsync((userManager.FindByEmailAsync(user.Email).Result)).Result.ToList();
            });

            return View(model);
        }
    }
}
