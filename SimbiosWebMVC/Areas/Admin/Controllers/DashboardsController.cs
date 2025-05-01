using Microsoft.AspNetCore.Mvc;

namespace SimbiosWebMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardsController : Controller
{
  public IActionResult Index() => View();
}
