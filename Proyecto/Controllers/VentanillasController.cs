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
    public class VentanillasController : Controller
    {
        private readonly ProyectoDBContext _context;

        public VentanillasController(ProyectoDBContext context) => _context = context;

        private Guid UsuarioActualId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var ventanillas = await _context.Ventanillas
                .Include(v => v.Sucursal)
                .Include(v => v.AsignacionVentanillas.Where(a => a.Hora_Fin == null && !a.Eliminado))
                    .ThenInclude(a => a.Empleado)
                .Include(v => v.VentanillaServicios.Where(vs => vs.Activo && !vs.Eliminado))
                    .ThenInclude(vs => vs.Servicio)
                .Where(v => !v.Eliminado)
                .OrderBy(v => v.Numero_Ventanilla)
                .ToListAsync();

            ViewBag.Sucursales = await _context.Sucursales.Where(s => !s.Eliminado).ToListAsync();
            ViewBag.Servicios  = await _context.Servicios.Where(s => !s.Eliminado).ToListAsync();
            ViewBag.Empleados  = await _context.Empleados
                .Where(e => !e.Eliminado)
                .OrderBy(e => e.Nombre).ThenBy(e => e.Apellido)
                .ToListAsync();

            return View("~/Views/Admin/Index.cshtml", ventanillas);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear(VentanillaVm vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Numero) || vm.SucursalId == Guid.Empty)
            {
                TempData["Error"] = "Número y sucursal son obligatorios.";
                return RedirectToAction(nameof(Index));
            }

            var userId = UsuarioActualId();

            var duplicado = await _context.Ventanillas
                .AnyAsync(v => !v.Eliminado && v.Numero_Ventanilla == vm.Numero);
            if (duplicado)
            {
                TempData["Error"] = $"Ya existe una ventanilla con el número '{vm.Numero}'.";
                return RedirectToAction(nameof(Index));
            }

            var ventanilla = Ventanilla.Crear(vm.Numero, vm.Estado, vm.SucursalId, userId);
            _context.Ventanillas.Add(ventanilla);

            if (vm.ServicioIds?.Length > 0)
                foreach (var sid in vm.ServicioIds)
                    _context.VentanillaServicios.Add(new VentanillaServicio
                    {
                        VentanillaServicioId = Guid.NewGuid(),
                        VentanillaId         = ventanilla.VentanillaId,
                        ServicioId           = sid,
                        Activo               = true,
                        CreateBy             = userId,
                        ModifiedBy           = userId
                    });

            if (vm.EmpleadoId != null && vm.EmpleadoId != Guid.Empty)
                _context.AsignacionVentanillas.Add(new AsignacionVentanilla
                {
                    AsignacionId = Guid.NewGuid(),
                    VentanillaId = ventanilla.VentanillaId,
                    EmpleadoId   = vm.EmpleadoId.Value,
                    Hora_Inicio  = DateTime.UtcNow,
                    CreateBy     = userId,
                    ModifiedBy   = userId
                });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Ventanilla creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar(VentanillaVm vm)
        {
            if (vm.VentanillaId == null)
            {
                TempData["Error"] = "ID inválido.";
                return RedirectToAction(nameof(Index));
            }

            var ventanilla = await _context.Ventanillas.FindAsync(vm.VentanillaId);
            if (ventanilla == null) return NotFound();

            var duplicado = await _context.Ventanillas
                .AnyAsync(v => !v.Eliminado && v.Numero_Ventanilla == vm.Numero && v.VentanillaId != vm.VentanillaId);
            if (duplicado)
            {
                TempData["Error"] = $"Ya existe una ventanilla con el número '{vm.Numero}'.";
                return RedirectToAction(nameof(Index));
            }

            var userId = UsuarioActualId();

            var existing = await _context.VentanillaServicios
                .Where(vs => vs.VentanillaId == vm.VentanillaId).ToListAsync();
            foreach (var e in existing) e.Eliminado = true;

            if (vm.ServicioIds?.Length > 0)
                foreach (var sid in vm.ServicioIds)
                {
                    var ex = existing.FirstOrDefault(e => e.ServicioId == sid);
                    if (ex != null) { ex.Eliminado = false; ex.Activo = true; }
                    else _context.VentanillaServicios.Add(new VentanillaServicio
                    {
                        VentanillaServicioId = Guid.NewGuid(),
                        VentanillaId         = vm.VentanillaId.Value,
                        ServicioId           = sid,
                        Activo               = true,
                        CreateBy             = userId,
                        ModifiedBy           = userId
                    });
                }

            var asignacionActiva = await _context.AsignacionVentanillas
                .FirstOrDefaultAsync(a => a.VentanillaId == vm.VentanillaId && a.Hora_Fin == null && !a.Eliminado);

            if (vm.EmpleadoId != null && vm.EmpleadoId != Guid.Empty)
            {
                if (asignacionActiva == null || asignacionActiva.EmpleadoId != vm.EmpleadoId)
                {
                    if (asignacionActiva != null)
                    {
                        asignacionActiva.Hora_Fin   = DateTime.UtcNow;
                        asignacionActiva.ModifiedBy = userId;
                    }
                    _context.AsignacionVentanillas.Add(new AsignacionVentanilla
                    {
                        AsignacionId = Guid.NewGuid(),
                        VentanillaId = vm.VentanillaId.Value,
                        EmpleadoId   = vm.EmpleadoId.Value,
                        Hora_Inicio  = DateTime.UtcNow,
                        CreateBy     = userId,
                        ModifiedBy   = userId
                    });
                }
            }
            else if (asignacionActiva != null)
            {
                asignacionActiva.Hora_Fin   = DateTime.UtcNow;
                asignacionActiva.ModifiedBy = userId;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Ventanilla actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Eliminar")]
        public async Task<IActionResult> Eliminar(VentanillaVm vm)
        {
            var ventanilla = await _context.Ventanillas.FindAsync(vm.VentanillaId);
            if (ventanilla != null)
            {
                ventanilla.EliminarLogico(UsuarioActualId());
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Ventanilla eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ─── AJAX Buscar ────────────
        [HttpPost("Buscar")]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> Buscar([FromBody] BusquedaVm? vm)
        {
            vm ??= new BusquedaVm();
            var q = _context.Ventanillas
                .Include(v => v.Sucursal)
                .Include(v => v.AsignacionVentanillas.Where(a => a.Hora_Fin == null && !a.Eliminado))
                    .ThenInclude(a => a.Empleado)
                .Include(v => v.VentanillaServicios.Where(vs => vs.Activo && !vs.Eliminado))
                    .ThenInclude(vs => vs.Servicio)
                .Where(v => !v.Eliminado);

            if (!string.IsNullOrWhiteSpace(vm.Busqueda))
                q = q.Where(v => v.Numero_Ventanilla.Contains(vm.Busqueda) ||
                                 (v.Sucursal != null && v.Sucursal.Nombre.Contains(vm.Busqueda)));

            if (!string.IsNullOrWhiteSpace(vm.Estado))
                q = q.Where(v => v.Estado_Ventanilla == vm.Estado);

            var lista = await q.OrderBy(v => v.Numero_Ventanilla).ToListAsync();

            var result = lista.Select(v =>
            {
                var emp = v.AsignacionVentanillas.FirstOrDefault();
                return new
                {
                    ventanillaId = v.VentanillaId,
                    numero       = v.Numero_Ventanilla,
                    sucursal     = v.Sucursal?.Nombre ?? "",
                    sucursalId   = v.SucursalId,
                    servicios    = string.Join(", ", v.VentanillaServicios.Select(vs => vs.Servicio?.Nombre_Servicio)),
                    servicioIds  = string.Join(",", v.VentanillaServicios.Select(vs => vs.ServicioId.ToString())),
                    empleado     = emp != null ? (emp.Empleado?.Nombre + " " + emp.Empleado?.Apellido).Trim() : "",
                    empleadoId   = emp?.EmpleadoId.ToString() ?? "",
                    estado       = v.Estado_Ventanilla
                };
            }).ToList();

            return new JsonResult(result);
        }
    }
}
