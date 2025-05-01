using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimbiosWebMVC.Areas.Admin.Models.User;
using SimbiosWebMVC.Constants;
using SimbiosWebMVC.Data.Entities.Identity;

namespace SimbiosWebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class UserController(UserManager<UserEntity> userManager, IMapper mapper) : Controller
    {
        [HttpGet]
        public IActionResult AllUsers() 
        {
            var userList = mapper.Map<List<UserItemViewModel>>(userManager.Users);

            return View(userList);
        }
    }
}
