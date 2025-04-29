using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimbiosWebMVC.Data.Entities.Identity;
using SimbiosWebMVC.Models.Account;
using System.Threading.Tasks;

namespace SimbiosWebMVC.ViewComponents
{
    public class UserLinkViewComponent (UserManager<UserEntity> userManager, IMapper mapper) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity?.Name;
            var model = new UserLinkViewModel();

            if (userName != null)
            {
                var user = userManager.FindByNameAsync(userName).Result;
                if (user != null)
                {
                    model = mapper.Map<UserLinkViewModel>(user);
                }
            }

            return View(model);
        }
    }
}
