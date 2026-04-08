using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Hubs;
using Proyecto.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.Controllers
{
    public class KioscoController : Controller
    {
        private readonly ProyectoDBContext _context;
        private readonly ITicketService   _ticketService;
        private readonly IHubContext<ColaC> _hubContext;

        public KioscoController(ProyectoDBContext context, ITicketService ticketService, IHubContext<ColaC> hubContext)
        {
            _context       = context;
            _ticketService = ticketService;
            _hubContext    = hubContext;
        }

        //pasos cliente
        // P1: Selección de servicio 
        public async Task<IActionResult> Index()
        {
            ViewData["HideNavbar"] = true;

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("ClienteNombre")))
                return RedirectToAction("Login", "Cliente");

            var servicios = await _context.Servicios
                .Where(s => s.Estado == "Activo" && !s.Eliminado)
                .ToListAsync();

            ViewBag.ClienteNombre = HttpContext.Session.GetString("ClienteNombreCompleto")
                                 ?? HttpContext.Session.GetString("ClienteNombre");
            return View(servicios);
        }

        // P2: Generar ticket (directo, sin pedir DNI)
        [HttpPost]
        public async Task<IActionResult> GenerarTicket(Guid servicioId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("ClienteNombre")))
                return RedirectToAction("Login", "Cliente");

            try
            {
                var sucursal = await _context.Sucursales
                    .Where(s => !s.Eliminado && s.Ventanillas.Any(v =>
                        !v.Eliminado &&
                        v.VentanillaServicios.Any(vs => vs.ServicioId == servicioId && vs.Activo && !vs.Eliminado)))
                    .FirstOrDefaultAsync();

                if (sucursal == null)
                    sucursal = await _context.Sucursales.FirstOrDefaultAsync(s => !s.Eliminado);

                if (sucursal == null)
                    return RedirectToAction("Index");

                Guid? clienteId = null;
                var clienteIdStr = HttpContext.Session.GetString("ClienteId");
                if (Guid.TryParse(clienteIdStr, out var parsedClienteId))
                    clienteId = parsedClienteId;

                var ticket = await _ticketService.GenerarTicketAsync(servicioId, sucursal.SucursalId, clienteId);

                var personasAntes = await _context.Tickets
                    .CountAsync(t => t.ServicioId  == servicioId
                                  && t.SucursalId  == sucursal.SucursalId
                                  && t.Estado_Ticket == "En espera"
                                  && t.Posicion    < ticket.Posicion
                                  && !t.Eliminado);

                await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate");

                return RedirectToAction(nameof(TicketGenerado), new
                {
                    numeroTicket  = ticket.Numero_Ticket,
                    servicio      = ticket.Servicio?.Nombre_Servicio,
                    personasAntes
                });
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.InnerException?.Message
                         ?? ex.InnerException?.Message
                         ?? ex.Message;
                TempData["Error"] = inner;
                return RedirectToAction("Index");
            }
        }

        // P3: Confirmación
        public IActionResult TicketGenerado(string numeroTicket, string servicio, int personasAntes)
        {
            ViewData["HideNavbar"] = true;
            ViewBag.NumeroTicket   = numeroTicket;
            ViewBag.Servicio       = servicio;
            ViewBag.PersonasAntes  = personasAntes;
            ViewBag.ClienteNombre  = HttpContext.Session.GetString("ClienteNombreCompleto")
                                  ?? HttpContext.Session.GetString("ClienteNombre");
            return View();
        }
    }
}
