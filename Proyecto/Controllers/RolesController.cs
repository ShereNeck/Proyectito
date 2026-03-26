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
    public class RolesController : Controller
    {
        private readonly ProyectoDBContext _context;

        public RolesController(ProyectoDBContext context) => _context = context;

        private Guid UsuarioActualId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles
                .Include(r => r.Usuarios.Where(u => !u.Eliminado))
                .Where(r => !r.Eliminado)
                .ToListAsync();

            return View("~/Views/Admin/Roles.cshtml", roles);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear(RolVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "El nombre del rol es obligatorio.";
                return RedirectToAction(nameof(Index));
            }

            var rol = Rol.Crear(vm.Nombre, vm.Descripcion ?? string.Empty, UsuarioActualId());
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Rol creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar(RolVm vm)
        {
            if (!ModelState.IsValid || vm.RolId == null)
            {
                TempData["Error"] = "Datos inválidos.";
                return RedirectToAction(nameof(Index));
            }

            var rol = await _context.Roles.FindAsync(vm.RolId);
            if (rol == null) return NotFound();

            rol.Actualizar(vm.Nombre, vm.Descripcion ?? string.Empty, UsuarioActualId());
            await _context.SaveChangesAsync();

            TempData["Success"] = "Rol actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Eliminar")]
        public async Task<IActionResult> Eliminar(RolVm vm)
        {
            var rol = await _context.Roles.FindAsync(vm.RolId);
            if (rol != null)
            {
                rol.EliminarLogico(UsuarioActualId());
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Rol eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        // ─── AJAX Buscar ─────
        [HttpPost("Buscar")]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> Buscar([FromBody] BusquedaVm? vm)
        {
            vm ??= new BusquedaVm();
            var q = _context.Roles
                .Include(r => r.Usuarios.Where(u => !u.Eliminado))
                .Where(r => !r.Eliminado);

            if (!string.IsNullOrWhiteSpace(vm.Busqueda))
                q = q.Where(r => r.Nombre.Contains(vm.Busqueda));

            if (!string.IsNullOrWhiteSpace(vm.Estado))
                q = q.Where(r => r.Estado == vm.Estado);

            var lista = await q.ToListAsync();

            var result = lista.Select(r => new
            {
                rolId = r.RolId,
                nombre = r.Nombre,
                descripcion = r.Descripcion ?? "",
                totalUsuarios = r.Usuarios.Count,
                estado = r.Estado
            });

            return new JsonResult(result);
        }
    }
}
