using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LearnSignalR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() { return View(); }
    }
}
