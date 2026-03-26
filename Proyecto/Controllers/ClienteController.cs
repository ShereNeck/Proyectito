using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Filters;
using Proyecto.Models;

namespace Proyecto.Controllers
{
	public class ClienteController : Controller
	{
		private readonly ProyectoDBContext _context;

		public ClienteController(ProyectoDBContext context)
		{
			_context = context;
		}


		[HttpGet]
		public IActionResult Login(string tab = "cliente")
		{
			if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ClienteNombre")))
				return RedirectToAction("Index", "Kiosco");

			return RedirectToAction("Login", "Account", new { tab });
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DoLogin(LoginClienteVm model)
		{
			if (!ModelState.IsValid)
			{
				TempData["LoginError"] = "Datos inválidos.";
				return RedirectToAction("Login", "Account", new { tab = "cliente" });
			}

			var usuario = await _context.Usuarios
				.Include(u => u.Rol)
				.FirstOrDefaultAsync(u =>
					u.Nombre == model.Username &&
					u.PasswordHash == model.Password &&
					!u.Eliminado);

			if (usuario == null || usuario.Rol?.Nombre != "Cliente")
			{
				TempData["LoginError"] = "Usuario o contraseña incorrectos.";
				return RedirectToAction("Login", "Account", new { tab = "cliente" });
			}

			HttpContext.Session.SetString("ClienteNombre",    usuario.Nombre);
			HttpContext.Session.SetString("ClienteUsuarioId", usuario.UsuarioId.ToString());
			return RedirectToAction("Index", "Kiosco");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Registrar(RegistroClienteVm model)
			=> await RedirectToAccountRegistrar(model);

		public IActionResult Logout()
		{
			HttpContext.Session.Remove("ClienteNombre");
			HttpContext.Session.Remove("ClienteNombreCompleto");
			HttpContext.Session.Remove("ClienteUsuarioId");
			return RedirectToAction("Login", "Account");
		}

		private async Task<IActionResult> RedirectToAccountRegistrar(RegistroClienteVm model)
		{
			TempData["RegisterError"] = "Por favor usa el formulario principal.";
			return RedirectToAction("Login", "Account", new { tab = "registro" });
		}
	}
}
