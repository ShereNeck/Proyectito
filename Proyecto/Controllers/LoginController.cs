using Microsoft.AspNetCore.Mvc;

namespace Proyecto.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
			=> RedirectToAction("Login", "Account");

		public IActionResult Logout()
			=> RedirectToAction("Logout", "Account");
	}
}
