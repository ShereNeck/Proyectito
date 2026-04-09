using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Data.Entidades;
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

        // GET: redirige al login de Account
        [HttpGet]
        public IActionResult Login(string tab = "cliente")
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ClienteNombre")))
                return RedirectToAction("Index", "Kiosco");
            return RedirectToAction("Login", "Account", new { tab });
        }

        // POST: Solo valida DNI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoLogin(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni) || dni.Length != 13)
            {
                TempData["LoginError"] = "Ingrese un DNI válido de 13 dígitos.";
                return RedirectToAction("Login", "Account", new { tab = "cliente" });
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.DNI == dni && !c.Eliminado);

            if (cliente == null)
            {
                // DNI no registrado, redirige a registro rápido
                TempData["DNIPendiente"] = dni;
                return RedirectToAction("Login", "Account", new { tab = "registro" });
            }

            // Sesión mínima
            HttpContext.Session.SetString("ClienteNombre", cliente.Nombre_Cliente);
            HttpContext.Session.SetString("ClienteId", cliente.ClienteId.ToString());
            return RedirectToAction("Index", "Kiosco");
        }

        // POST: Registro rápido — solo DNI, Nombre, Apellido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroRapido(string dni, string nombre, string apellido)
        {
            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
            {
                TempData["RegisterError"] = "Todos los campos son obligatorios.";
                return RedirectToAction("Login", "Account", new { tab = "registro" });
            }

            // Verificar que el DNI no exista ya
            var existe = await _context.Clientes.AnyAsync(c => c.DNI == dni && !c.Eliminado);
            if (existe)
            {
                TempData["LoginError"] = "Este DNI ya está registrado. Ingrese su DNI nuevamente.";
                return RedirectToAction("Login", "Account", new { tab = "cliente" });
            }

            var cliente = new Cliente
            {
                ClienteId = Guid.NewGuid(),
                DNI = dni,
                Nombre_Cliente = nombre,
                Apellido_Cliente = apellido,
                Estado = "Activo"
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("ClienteNombre", cliente.Nombre_Cliente);
            HttpContext.Session.SetString("ClienteId", cliente.ClienteId.ToString());
            return RedirectToAction("Index", "Kiosco");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("ClienteNombre");
            HttpContext.Session.Remove("ClienteId");
            return RedirectToAction("Login", "Account");
        }
    }
}