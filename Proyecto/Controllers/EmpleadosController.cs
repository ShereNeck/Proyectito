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
    public class EmpleadosController : Controller
    {
        private readonly ProyectoDBContext _context;

        public EmpleadosController(ProyectoDBContext context) => _context = context;

        private Guid UsuarioActualId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var empleados = await _context.Empleados
                .Include(e => e.Usuario).ThenInclude(u => u.Rol)
                .Where(e => !e.Eliminado)
                .OrderBy(e => e.Nombre)
                .ToListAsync();

            ViewBag.Roles = await _context.Roles.Where(r => !r.Eliminado).ToListAsync();
            ViewBag.Servicios = await _context.Servicios
                .Where(s => !s.Eliminado && s.Estado == "Activo")
                .OrderBy(s => s.Nombre_Servicio)
                .ToListAsync();

            return View("~/Views/Admin/Empleados.cshtml", empleados);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear(EmpleadoVm vm)
        {
            var userId = UsuarioActualId();

            if (vm.RolId == null || vm.RolId == Guid.Empty)
            {
                TempData["Error"] = "Debe seleccionar un rol válido.";
                return RedirectToAction(nameof(Index));
            }

            if (await _context.Usuarios.AnyAsync(u => u.Nombre == vm.NombreUsuario && !u.Eliminado))
            {
                TempData["Error"] = "Ya existe un usuario con ese nombre de usuario.";
                return RedirectToAction(nameof(Index));
            }

            var rol = await _context.Roles.FindAsync(vm.RolId);
            if (rol?.Nombre == "Agente Bancario" && string.IsNullOrWhiteSpace(vm.Cargo))
            {
                TempData["Error"] = "Debe seleccionar el servicio que atenderá el agente.";
                return RedirectToAction(nameof(Index));
            }

            var usuarioId = Guid.NewGuid();
            _context.Usuarios.Add(new Usuario
            {
                UsuarioId    = usuarioId,
                Nombre       = vm.NombreUsuario!,
                Email        = vm.NombreUsuario!.ToLower().Replace(" ", "") + "@banco.com",
                PasswordHash = vm.Contrasena!,
                RolId        = vm.RolId.Value,
                CreateBy     = userId,
                ModifiedBy   = userId
            });

            _context.Empleados.Add(new Empleado
            {
                EmpleadoId = Guid.NewGuid(),
                Nombre     = vm.Nombre,
                Apellido   = vm.Apellido,
                Cargo      = string.IsNullOrWhiteSpace(vm.Cargo) ? null : vm.Cargo,
                UsuarioId  = usuarioId,
                CreateBy   = userId,
                ModifiedBy = userId
            });

            await _context.SaveChangesAsync();
            TempData["Success"] = "Empleado creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar(EmpleadoVm vm)
        {
            if (vm.EmpleadoId == null)
            {
                TempData["Error"] = "ID inválido.";
                return RedirectToAction(nameof(Index));
            }

            var empleado = await _context.Empleados.FindAsync(vm.EmpleadoId);
            if (empleado == null) return NotFound();

            empleado.Actualizar(vm.Nombre, vm.Apellido, vm.Cargo ?? string.Empty, UsuarioActualId());
            await _context.SaveChangesAsync();

            TempData["Success"] = "Empleado actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Eliminar")]
        public async Task<IActionResult> Eliminar(EmpleadoVm vm)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.EmpleadoId == vm.EmpleadoId);

            if (empleado != null)
            {
                var userId = UsuarioActualId();
                empleado.EliminarLogico(userId);
                if (empleado.Usuario != null) empleado.Usuario.EliminarLogico(userId);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Empleado eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ─── AJAX Buscar ───
        [HttpPost("Buscar")]
        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> Buscar([FromBody] BusquedaVm? vm)
        {
            vm ??= new BusquedaVm();
            var q = _context.Empleados
                .Include(e => e.Usuario).ThenInclude(u => u!.Rol)
                .Where(e => !e.Eliminado);

            if (!string.IsNullOrWhiteSpace(vm.Busqueda))
                q = q.Where(e => e.Nombre.Contains(vm.Busqueda) ||
                                 e.Apellido.Contains(vm.Busqueda) ||
                                 (e.Usuario != null && e.Usuario.Nombre.Contains(vm.Busqueda)));

            if (vm.RolId.HasValue && vm.RolId != Guid.Empty)
                q = q.Where(e => e.Usuario != null && e.Usuario.RolId == vm.RolId);

            var lista = await q.OrderBy(e => e.Nombre).ThenBy(e => e.Apellido).ToListAsync();

            var result = lista.Select(e => new
            {
                empleadoId = e.EmpleadoId,
                nombre     = e.Nombre,
                apellido   = e.Apellido,
                cargo      = e.Cargo ?? "",
                usuario    = e.Usuario?.Nombre ?? "",
                rol        = e.Usuario?.Rol?.Nombre ?? "",
                esAdmin    = (e.Usuario?.Rol?.Nombre ?? "").ToLower().Contains("admin")
            });

            return new JsonResult(result);
        }
    }
}
