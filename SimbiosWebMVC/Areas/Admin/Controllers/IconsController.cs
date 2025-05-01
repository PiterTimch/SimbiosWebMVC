using Microsoft.AspNetCore.Mvc;

namespace SimbiosWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class IconsController : Controller
{
  public IActionResult RiIcons() => View();
}
