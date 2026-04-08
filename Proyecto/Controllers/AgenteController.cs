using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Hubs;
using Proyecto.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Proyecto.Controllers
{
    [Authorize(Roles = "Agente")]
    public class AgenteController : Controller
    {
        private readonly ProyectoDBContext _context;
        private readonly IAgentService _agentService;
        private readonly ITicketService _ticketService;
        private readonly IHubContext<ColaC> _hubContext;

        public AgenteController(ProyectoDBContext context, IAgentService agentService, ITicketService ticketService, IHubContext<ColaC> hubContext)
        {
            _context = context;
            _agentService = agentService;
            _ticketService = ticketService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return RedirectToAction("Index", "Login");



            var asignacion = await _context.AsignacionVentanillas
                .Include(a => a.Ventanilla)
                .FirstOrDefaultAsync(a => a.Empleado.UsuarioId == userId && a.Hora_Fin == null && !a.Eliminado);


            if (asignacion == null)
            {
                var empleado = await _context.Empleados
                    .FirstOrDefaultAsync(e => e.UsuarioId == userId && !e.Eliminado);

                if (empleado != null && !string.IsNullOrWhiteSpace(empleado.Cargo))
                {


                    var ventanilla = await _context.Ventanillas
                        .Include(v => v.VentanillaServicios.Where(vs => vs.Activo && !vs.Eliminado))
                            .ThenInclude(vs => vs.Servicio)
                        .Where(v => !v.Eliminado &&
                                    v.VentanillaServicios.Any(vs => vs.Activo && !vs.Eliminado &&
                                        vs.Servicio.Nombre_Servicio == empleado.Cargo))
                        .FirstOrDefaultAsync();

                    if (ventanilla != null)
                    {
                        asignacion = new Proyecto.Data.Entidades.AsignacionVentanilla
                        {
                            AsignacionId = Guid.NewGuid(),
                            EmpleadoId   = empleado.EmpleadoId,
                            VentanillaId = ventanilla.VentanillaId,
                            Hora_Inicio  = DateTime.UtcNow,
                            CreateBy     = userId,
                            ModifiedBy   = userId
                        };
                        _context.AsignacionVentanillas.Add(asignacion);
                        await _context.SaveChangesAsync();
                        asignacion.Ventanilla = ventanilla;
                    }
                }
            }

            if (asignacion == null)
            {
                ViewBag.Error = "No se encontró una ventanilla disponible para su servicio. Contacte al administrador.";
                return View();
            }

            ViewBag.VentanillaName = asignacion.Ventanilla.Numero_Ventanilla;
            ViewBag.VentanillaId = asignacion.VentanillaId;

            // Fetch current ticket
            var currentTicket = await _ticketService.ObtenerTicketActualPorVentanillaAsync(asignacion.VentanillaId);
            ViewBag.CurrentTicket = currentTicket;

            // Fetch pending queue
            var serviciosAsignados = await _context.VentanillaServicios
                .Where(vs => vs.VentanillaId == asignacion.VentanillaId && vs.Activo && !vs.Eliminado)
                .Select(vs => vs.ServicioId).ToListAsync();

            var pendingQueue = await _context.Tickets
                .Include(t => t.Servicio)
                .Include(t => t.Cola).ThenInclude(c => c.Prioridad)
                .Where(t => t.Estado_Ticket == "En espera" && serviciosAsignados.Contains(t.ServicioId) && t.SucursalId == asignacion.Ventanilla.SucursalId && !t.Eliminado)
                .OrderByDescending(t => t.Cola.Prioridad.Peso)
                .ThenBy(t => t.Hora_Emision)
                .ToListAsync();

            return View(pendingQueue);
        }

        [HttpPost]
        public async Task<IActionResult> LlamarSiguiente(Guid ventanillaId)
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.UsuarioId == Guid.Parse(userIdStr));
                if (empleado == null) return BadRequest();

                var ticket = await _agentService.LlamarSiguienteAsync(ventanillaId, empleado.EmpleadoId);
                if (ticket != null)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarTicket(Guid ticketId)
        {
            try
            {
                await _agentService.FinalizarTicketAsync(ticketId);
                await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MarcarAusente(Guid ticketId)
        {
            try
            {
                await _agentService.MarcarAusenteAsync(ticketId);
                await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PromoverTicket(Guid ticketId)
        {
            try
            {
                var ticket = await _context.Tickets
                    .Include(t => t.Cola)
                    .FirstOrDefaultAsync(t => t.TicketId == ticketId && !t.Eliminado);

                if (ticket == null) return NotFound();

                var preferencial = await _context.Prioridades
                    .FirstOrDefaultAsync(p => p.Nombre == "Preferencial" && !p.Eliminado);

                if (preferencial == null)
                {
                    TempData["Error"] = "No se encontró la prioridad Preferencial en el sistema.";
                    return RedirectToAction("Index");
                }

                ticket.Cola.PrioridadId = preferencial.PrioridadId;
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> LlamarDeNuevo(Guid ticketId)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarYSiguiente(Guid ticketId, Guid ventanillaId)
        {
            try
            {
                await _agentService.FinalizarTicketAsync(ticketId);
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.UsuarioId == Guid.Parse(userIdStr));
                if (empleado != null)
                    await _agentService.LlamarSiguienteAsync(ventanillaId, empleado.EmpleadoId);
                await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
