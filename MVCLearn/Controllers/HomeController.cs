using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
