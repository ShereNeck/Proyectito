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
    public class ServiciosController : Controller
    {
        private readonly ProyectoDBContext _context;

        public ServiciosController(ProyectoDBContext context) => _context = context;

        private Guid UsuarioActualId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var servicios = await _context.Servicios
                .Where(s => !s.Eliminado)
                .OrderBy(s => s.Nombre_Servicio)
                .ToListAsync();

            return View("~/Views/Admin/Servicios.cshtml", servicios);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear(ServicioVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos. Revisa el formulario.";
                return RedirectToAction(nameof(Index));
            }

            var nombreDup = await _context.Servicios
                .AnyAsync(s => !s.Eliminado && s.Nombre_Servicio == vm.Nombre);
            if (nombreDup)
            {
                TempData["Error"] = $"Ya existe un servicio con el nombre '{vm.Nombre}'.";
                return RedirectToAction(nameof(Index));
            }

            var prefijoDup = await _context.Servicios
                .AnyAsync(s => !s.Eliminado && s.Prefijo_Ticket == vm.Prefijo);
            if (prefijoDup)
            {
                TempData["Error"] = $"Ya existe un servicio con el prefijo '{vm.Prefijo}'.";
                return RedirectToAction(nameof(Index));
            }

            var servicio = Servicio.Crear(vm.Nombre, vm.Prefijo, vm.Descripcion,
                                          vm.TiempoEstimado, vm.Estado, UsuarioActualId());
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Servicio creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar(ServicioVm vm)
        {
            if (!ModelState.IsValid || vm.ServicioId == null)
            {
                TempData["Error"] = "Datos inválidos.";
                return RedirectToAction(nameof(Index));
            }

            var servicio = await _context.Servicios.FindAsync(vm.ServicioId);
            if (servicio == null) return NotFound();

            var nombreDup = await _context.Servicios
                .AnyAsync(s => !s.Eliminado && s.Nombre_Servicio == vm.Nombre && s.ServicioId != vm.ServicioId);
            if (nombreDup)
            {
                TempData["Error"] = $"Ya existe un servicio con el nombre '{vm.Nombre}'.";
                return RedirectToAction(nameof(Index));
            }

            var prefijoDup = await _context.Servicios
                .AnyAsync(s => !s.Eliminado && s.Prefijo_Ticket == vm.Prefijo && s.ServicioId != vm.ServicioId);
            if (prefijoDup)
            {
                TempData["Error"] = $"Ya existe un servicio con el prefijo '{vm.Prefijo}'.";
                return RedirectToAction(nameof(Index));
            }

            servicio.Actualizar(vm.Nombre, vm.Prefijo, vm.Descripcion,
                                vm.TiempoEstimado, vm.Estado, UsuarioActualId());
            await _context.SaveChangesAsync();

            TempData["Success"] = "Servicio actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Eliminar")]
        public async Task<IActionResult> Eliminar(ServicioVm vm)
        {
            var servicio = await _context.Servicios.FindAsync(vm.ServicioId);
            if (servicio != null)
            {
                servicio.EliminarLogico(UsuarioActualId());
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Servicio eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ─── AJAX Buscar ─────
        [HttpPost("Buscar")]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> Buscar([FromBody] BusquedaVm? vm)
        {
            vm ??= new BusquedaVm();
            var q = _context.Servicios.Where(s => !s.Eliminado);

            if (!string.IsNullOrWhiteSpace(vm.Busqueda))
                q = q.Where(s => s.Nombre_Servicio.Contains(vm.Busqueda) ||
                                 s.Prefijo_Ticket.Contains(vm.Busqueda));

            if (!string.IsNullOrWhiteSpace(vm.Estado))
                q = q.Where(s => s.Estado == vm.Estado);

            var result = await q.OrderBy(s => s.Nombre_Servicio)
                .Select(s => new
                {
                    servicioId     = s.ServicioId,
                    nombre         = s.Nombre_Servicio,
                    prefijo        = s.Prefijo_Ticket,
                    descripcion    = s.Descripcion ?? "",
                    tiempoEstimado = s.Tiempo_Estimado,
                    estado         = s.Estado
                }).ToListAsync();

            return new JsonResult(result);
        }
    }
}
