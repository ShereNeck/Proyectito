using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Data.Entidades;

namespace Proyecto.Services
{
    public interface IAgentService
    {
        Task<Ticket> LlamarSiguienteAsync(Guid ventanillaId, Guid empleadoId);
        Task<bool> FinalizarTicketAsync(Guid ticketId);
        Task<bool> MarcarAusenteAsync(Guid ticketId);
    }

    public class AgentService : IAgentService
    {
        private readonly ProyectoDBContext _context;

        public AgentService(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<Ticket> LlamarSiguienteAsync(Guid ventanillaId, Guid empleadoId)
        {
            // 1. Validar ventanilla activa
            var ventanilla = await _context.Ventanillas
                .Include(v => v.VentanillaServicios)
                .FirstOrDefaultAsync(v => v.VentanillaId == ventanillaId && v.Estado_Ventanilla == "Activa" && !v.Eliminado);

            if (ventanilla == null)
                throw new Exception("La ventanilla no existe o está cerrada.");

            // 2. Validar que el empleado esté asignado a ESTA ventanilla y a ninguna otra
            var asignacionActiva = await _context.AsignacionVentanillas
                .Where(a => a.EmpleadoId == empleadoId && a.Hora_Fin == null && !a.Eliminado)
                .ToListAsync();

            if (!asignacionActiva.Any(a => a.VentanillaId == ventanillaId))
                throw new Exception("El empleado no está asignado a esta ventanilla.");

            if (asignacionActiva.Count > 1)
                throw new Exception("El empleado no puede estar asignado a múltiples ventanillas simultáneamente.");

            // 3. Validar que la ventanilla no esté ya atendiendo a alguien
            var ticketEnAtencion = await _context.Tickets
                .FirstOrDefaultAsync(t => t.VentanillaId == ventanillaId && t.Estado_Ticket == "En atención" && !t.Eliminado);

            if (ticketEnAtencion != null)
                throw new Exception("No se puede atender dos tickets al mismo tiempo en la misma ventanilla.");

            // 4. Obtener servicios permitidos para esta ventanilla
            var serviciosPermitidos = ventanilla.VentanillaServicios
                .Where(vs => vs.Activo && !vs.Eliminado)
                .Select(vs => vs.ServicioId)
                .ToList();

            if (!serviciosPermitidos.Any())
                throw new Exception("No hay servicios asignados a esta ventanilla.");

            // 5. Buscar el próximo ticket (prioridad más alta, y fecha más antigua)
            var proximoTicket = await _context.Tickets
                .Include(t => t.Cola).ThenInclude(c => c.Prioridad)
                .Where(t => t.SucursalId == ventanilla.SucursalId 
                            && t.Estado_Ticket == "En espera" 
                            && serviciosPermitidos.Contains(t.ServicioId) 
                            && !t.Eliminado)
                .OrderByDescending(t => t.Cola.Prioridad.Peso) // FIFO Priority
                .ThenBy(t => t.Hora_Emision) // FIFO Time
                .FirstOrDefaultAsync();

            if (proximoTicket == null)
                return null; // No hay tickets en espera

            // 6. Asignar ticket a la ventanilla y marcar en atención
            proximoTicket.Estado_Ticket = "En atención";
            proximoTicket.VentanillaId = ventanillaId;
            proximoTicket.Hora_Atencion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return proximoTicket;
        }

        public async Task<bool> FinalizarTicketAsync(Guid ticketId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId && !t.Eliminado);
            if (ticket == null) return false;

            if (ticket.Estado_Ticket != "En atención")
                throw new Exception("No se puede finalizar un ticket que no está en estado 'En atención'.");

            ticket.Estado_Ticket = "Finalizado";
            ticket.Hora_Finalizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarcarAusenteAsync(Guid ticketId)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == ticketId && !t.Eliminado);
            if (ticket == null) return false;

            if (ticket.Estado_Ticket != "En atención" && ticket.Estado_Ticket != "En espera")
                throw new Exception("No se puede marcar ausente un ticket en este estado.");

            ticket.Estado_Ticket = "Ausente";
            ticket.Hora_Finalizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
