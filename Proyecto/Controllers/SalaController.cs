using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.Controllers
{
    public class SalaController : Controller
    {
        private readonly ProyectoDBContext _context;

        public SalaController(ProyectoDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Kiosco");
        }

        public async Task<IActionResult> GetQueueData()
        {
             var currentTicket = await _context.Tickets.Include(t => t.Servicio).Include(t => t.Ventanilla).Where(t => t.Estado_Ticket == "En atención" && !t.Eliminado).OrderByDescending(t => t.Hora_Atencion)
                .Select(t => new { 
                    numeroTicket = t.Numero_Ticket, 
                    ventanilla = t.Ventanilla.Numero_Ventanilla, 
                    servicio = t.Servicio.Nombre_Servicio 
                }).FirstOrDefaultAsync();

            var nextTickets = await _context.Tickets
                .Include(t => t.Servicio)
                .Include(t => t.Cola).ThenInclude(c => c.Prioridad)
                .Include(t => t.Ventanilla)
                .Where(t => t.Estado_Ticket == "En espera" || (t.Estado_Ticket == "En atención" && t.TicketId.ToString() != (currentTicket != null ? _context.Tickets.OrderByDescending(x => x.Hora_Atencion).FirstOrDefault(x => x.Estado_Ticket == "En atención").TicketId.ToString() : "")))
                .OrderByDescending(t => t.Estado_Ticket == "En atención" ? 1 : 0)
                .ThenByDescending(t => t.Cola.Prioridad.Peso)
                .ThenBy(t => t.Hora_Emision)
                .Take(4)
                .Select(t => new {
                    numeroTicket = t.Numero_Ticket,
                    servicio = t.Servicio.Nombre_Servicio,
                    ventanilla = t.Ventanilla != null ? t.Ventanilla.Numero_Ventanilla : "En espera"
                })
                .ToListAsync();

            return Json(new { current = currentTicket, queue = nextTickets });
        }
    }
}
