using Microsoft.AspNetCore.Mvc;

namespace SimbiosWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class CardsController : Controller
{
  public IActionResult Basic() => View();
}
