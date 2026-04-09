using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
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

        // ── LANDING / LOGIN ────────────────────────────────────────────

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

        // ── LOGIN EMPLEADO ─────────────────────────────────────────────

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

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return rolNombre == "Administrador"
                ? RedirectToAction("Index", "Admin")
                : RedirectToAction("Index", "Agente");
        }

        // ── LOGOUT ────────────────────────────────────────────────────

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("ClienteNombre");
            HttpContext.Session.Remove("ClienteId");
            return RedirectToAction("Login");
        }

        // ── ACCESO DENEGADO ───────────────────────────────────────────

        public IActionResult AccesoDenegado(string? returnUrl = null)
        {
            ViewData["HideNavbar"] = true;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}