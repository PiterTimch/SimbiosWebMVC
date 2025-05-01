using Microsoft.AspNetCore.Mvc;

namespace SimbiosWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class TablesController : Controller
{
  public IActionResult Basic() => View();
}
