using Microsoft.AspNetCore.Mvc;

namespace Elsa.Dashboard.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {

      return RedirectToPage("/_Host");
    }
  }
}
