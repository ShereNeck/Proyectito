using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;
using System.Security.Claims;

namespace Proyecto.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("Admin/[controller]")]
    public class ClientesController : Controller
    {
        private readonly ProyectoDBContext _context;

        public ClientesController(ProyectoDBContext context) => _context = context;

        private Guid UsuarioActualId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .Where(c => !c.Eliminado)
                .OrderBy(c => c.Apellido_Cliente).ThenBy(c => c.Nombre_Cliente)
                .ToListAsync();

            return View("~/Views/Admin/Clientes.cshtml", clientes);
        }

        [HttpPost("EditarTipo")]
        public async Task<IActionResult> EditarTipo(ClienteVm vm)
        {
            if (vm.ClienteId == null)
            {
                TempData["Error"] = "ID de cliente inválido.";
                return RedirectToAction(nameof(Index));
            }

            var cliente = await _context.Clientes.FindAsync(vm.ClienteId);
            if (cliente == null) return NotFound();

            cliente.TipoCliente  = vm.TipoCliente ?? "Normal";
            cliente.ModifiedBy   = UsuarioActualId();
            cliente.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Tipo de cliente actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Eliminar")]
        public async Task<IActionResult> Eliminar(ClienteVm vm)
        {
            var cliente = await _context.Clientes.FindAsync(vm.ClienteId);
            if (cliente != null)
            {
                var userId = UsuarioActualId();
                cliente.Eliminado    = true;
                cliente.ModifiedBy   = userId;
                cliente.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Cliente eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ─── AJAX Buscar ───────────────────────────────────────────────
        [HttpPost("Buscar")]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> Buscar([FromBody] BusquedaClienteVm? vm)
        {
            vm ??= new BusquedaClienteVm();
            var q = _context.Clientes.Where(c => !c.Eliminado);

            if (!string.IsNullOrWhiteSpace(vm.Busqueda))
                q = q.Where(c =>
                    c.Nombre_Cliente.Contains(vm.Busqueda) ||
                    c.Apellido_Cliente.Contains(vm.Busqueda) ||
                    c.DNI.Contains(vm.Busqueda));

            if (!string.IsNullOrWhiteSpace(vm.TipoCliente))
                q = q.Where(c => c.TipoCliente == vm.TipoCliente);

            var lista = await q
                .OrderBy(c => c.Apellido_Cliente).ThenBy(c => c.Nombre_Cliente)
                .ToListAsync();

            var result = lista.Select(c => new
            {
                clienteId   = c.ClienteId,
                dni         = c.DNI,
                nombre      = c.Nombre_Cliente,
                apellido    = c.Apellido_Cliente,
                tipoCliente = c.TipoCliente ?? "Normal",
                estado      = c.Estado ?? "Activo"
            });

            return new JsonResult(result);
        }
    }

    // ─── VM for the search ─────────────────────────────────────────────
    public class BusquedaClienteVm
    {
        public string? Busqueda    { get; set; }
        public string? TipoCliente { get; set; }
    }
}
