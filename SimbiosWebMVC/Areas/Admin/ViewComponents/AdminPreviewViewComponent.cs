using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimbiosWebMVC.Areas.Admin.Models.ViewComponentModels;
using SimbiosWebMVC.Data.Entities.Identity;
using SimbiosWebMVC.Models.Account;

namespace SimbiosWebMVC.Areas.Admin.ViewComponents
{
    public class AdminPreviewViewComponent(UserManager<UserEntity> userManager, IMapper mapper) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity?.Name;
            var model = new AdminPreviewModel();

            if (userName != null)
            {
                var user = userManager.FindByNameAsync(userName).Result;
                if (user != null)
                {
                    model = mapper.Map<AdminPreviewModel>(user);
                }
            }

            return View(model);
        }
    }
}
