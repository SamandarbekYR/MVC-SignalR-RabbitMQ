using Microsoft.AspNetCore.Mvc;
using MVCLearn.DataAcess.Data;
using MVCLearn.DataAcess.Interfaces.Users;

namespace MVCLearn.Controllers
{
    public class AccauntController : Controller
    {
        private IUserRepository _service;

        public AccauntController(IUserRepository service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Gmail)
        {
            var user =  _service.GetAll().FirstOrDefault(e => e.Gmail == Gmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Foydalanuvchi topilmadi.");
                return View();
            }

            if (user.RoleName == "Boss")
            {
                return RedirectToAction("Boss", "Messages");
            }

            else if (user.RoleName == "Worker")
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                return RedirectToAction("Worker", "Messages");
            } 

            ModelState.AddModelError("", "Noma'lum rol.");
            return View();
        }
    }
}
