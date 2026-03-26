using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Data.Entidades;
using Proyecto.Models;
using System.Security.Claims;

namespace Proyecto.Controllers
{
	public class AccountController : Controller
	{
		private readonly ProyectoDBContext _context;

		public AccountController(ProyectoDBContext context)
		{
			_context = context;
		}


		[HttpGet]
		public IActionResult Login(string tab = "")
		{

			if (User.Identity?.IsAuthenticated == true)
			{
				var rol = User.FindFirstValue(ClaimTypes.Role) ?? "";
				return rol == "Administrador"
					? RedirectToAction("Index", "Admin")
					: RedirectToAction("Index", "Agente");
			}

			if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ClienteNombre")))
				return RedirectToAction("Index", "Kiosco");

			ViewData["HideNavbar"] = true;
			ViewBag.Tab = tab;
			return View(new LoginEmpleadoVm());
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LoginEmpleado(LoginEmpleadoVm model)
		{
			ViewData["HideNavbar"] = true;
			ViewBag.Tab = "empleado";

			if (!ModelState.IsValid)
				return View("Login", model);

			var usuario = await _context.Usuarios
				.Include(u => u.Rol)
				.FirstOrDefaultAsync(u =>
					u.Nombre == model.Username &&
					u.PasswordHash == model.Password &&
					!u.Eliminado);

			if (usuario == null || usuario.Rol == null)
			{
				ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
				return View("Login", model);
			}

			var rolNombre = usuario.Rol.Nombre;

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name,           usuario.Nombre),
				new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
				new Claim(ClaimTypes.Role,           rolNombre)
			};

			var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(identity));

			return rolNombre == "Administrador"
				? RedirectToAction("Index", "Admin")
				: RedirectToAction("Index", "Agente");
		}

	
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LoginCliente(LoginClienteVm model)
		{
			if (!ModelState.IsValid)
			{
				TempData["LoginError"] = "Datos inválidos.";
				return RedirectToAction("Login", new { tab = "cliente" });
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
				return RedirectToAction("Login", new { tab = "cliente" });
			}

			HttpContext.Session.SetString("ClienteNombre",    usuario.Nombre);
			HttpContext.Session.SetString("ClienteUsuarioId", usuario.UsuarioId.ToString());

			return RedirectToAction("Index", "Kiosco");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegistrarCliente(RegistroClienteVm model)
		{
			ViewData["HideNavbar"] = true;
			ViewBag.Tab = "registro";

			if (!ModelState.IsValid)
				return View("Login", model);

			if (await _context.Usuarios.AnyAsync(u => u.Nombre == model.Username && !u.Eliminado))
			{
				ModelState.AddModelError(nameof(model.Username), "Ese nombre de usuario ya está en uso.");
				return View("Login", model);
			}

			if (await _context.Clientes.AnyAsync(c => c.DNI == model.Dni && !c.Eliminado))
			{
				ModelState.AddModelError(nameof(model.Dni), "Ya existe una cuenta registrada con ese DNI.");
				return View("Login", model);
			}

			var rolCliente = await _context.Roles
				.FirstOrDefaultAsync(r => r.Nombre == "Cliente" && !r.Eliminado);

			if (rolCliente == null)
			{
				rolCliente = new Rol
				{
					RolId       = Guid.NewGuid(),
					Nombre      = "Cliente",
					Descripcion = "Usuario cliente del sistema de turnos",
					Estado      = "Activo",
					CreateBy    = Guid.Empty,
					ModifiedBy  = Guid.Empty
				};
				_context.Roles.Add(rolCliente);
				await _context.SaveChangesAsync();
			}

			var usuarioId = Guid.NewGuid();

			_context.Usuarios.Add(new Usuario
			{
				UsuarioId    = usuarioId,
				Nombre       = model.Username,
				Email        = model.Email ?? string.Empty,
				PasswordHash = model.Password,
				RolId        = rolCliente.RolId,
				CreateBy     = Guid.Empty,
				ModifiedBy   = Guid.Empty
			});

			_context.Clientes.Add(new Cliente
			{
				ClienteId        = Guid.NewGuid(),
				DNI              = model.Dni,
				Nombre_Cliente   = model.Nombre,
				Apellido_Cliente = model.Apellido,
				Fecha_Nacimiento = model.FechaNacimiento,
				Estado           = "Activo",
				CreateBy         = Guid.Empty,
				ModifiedBy       = Guid.Empty
			});

			await _context.SaveChangesAsync();

			HttpContext.Session.SetString("ClienteNombre",         model.Username);
			HttpContext.Session.SetString("ClienteNombreCompleto", model.Nombre + " " + model.Apellido);
			HttpContext.Session.SetString("ClienteUsuarioId",      usuarioId.ToString());

			return RedirectToAction("Index", "Kiosco");
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Remove("ClienteNombre");
			HttpContext.Session.Remove("ClienteNombreCompleto");
			HttpContext.Session.Remove("ClienteUsuarioId");
			return RedirectToAction("Login");
		}


		public IActionResult AccesoDenegado(string? returnUrl = null)
		{
			ViewData["HideNavbar"] = true;
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}
	}
}
