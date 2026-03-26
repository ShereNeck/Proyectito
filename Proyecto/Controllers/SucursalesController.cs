using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Data.Entidades;
using Proyecto.Models;
using System.Security.Claims;

namespace Proyecto.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("Admin/[controller]")]
    public class SucursalesController : Controller
    {
        private readonly ProyectoDBContext _context;

        public SucursalesController(ProyectoDBContext context) => _context = context;

        private Guid UsuarioActualId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var sucursales = await _context.Sucursales
                .Where(s => !s.Eliminado)
                .OrderBy(s => s.Nombre)
                .ToListAsync();

            return View("~/Views/Admin/Sucursales.cshtml", sucursales);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear(SucursalVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos. Revisa el formulario.";
                return RedirectToAction(nameof(Index));
            }

            var sucursal = Sucursal.Crear(vm.Nombre, vm.Direccion, vm.Telefono,
                                          vm.Estado, UsuarioActualId());
            _context.Sucursales.Add(sucursal);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Sucursal creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar(SucursalVm vm)
        {
            if (!ModelState.IsValid || vm.SucursalId == null)
            {
                TempData["Error"] = "Datos inválidos.";
                return RedirectToAction(nameof(Index));
            }

            var sucursal = await _context.Sucursales.FindAsync(vm.SucursalId);
            if (sucursal == null) return NotFound();

            sucursal.Actualizar(vm.Nombre, vm.Direccion, vm.Telefono,
                                vm.Estado, UsuarioActualId());
            await _context.SaveChangesAsync();

            TempData["Success"] = "Sucursal actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Eliminar")]
        public async Task<IActionResult> Eliminar(SucursalVm vm)
        {
            var sucursal = await _context.Sucursales.FindAsync(vm.SucursalId);
            if (sucursal != null)
            {
                sucursal.EliminarLogico(UsuarioActualId());
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Sucursal eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ─── AJAX Buscar ──────────
        [HttpPost("Buscar")]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> Buscar([FromBody] BusquedaVm? vm)
        {
            vm ??= new BusquedaVm();
            var q = _context.Sucursales.Where(s => !s.Eliminado);

            if (!string.IsNullOrWhiteSpace(vm.Busqueda))
                q = q.Where(s => s.Nombre.Contains(vm.Busqueda) ||
                                 s.Direccion.Contains(vm.Busqueda));

            if (!string.IsNullOrWhiteSpace(vm.Estado))
                q = q.Where(s => s.Estado == vm.Estado);

            var result = await q.OrderBy(s => s.Nombre)
                .Select(s => new
                {
                    sucursalId = s.SucursalId,
                    nombre     = s.Nombre,
                    direccion  = s.Direccion,
                    telefono   = s.Telefono ?? "",
                    estado     = s.Estado
                }).ToListAsync();

            return new JsonResult(result);
        }
    }
}
