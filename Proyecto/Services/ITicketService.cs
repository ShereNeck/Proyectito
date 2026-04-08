using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Data.Entidades;

namespace Proyecto.Services
{
    public interface ITicketService
    {
        Task<Ticket> GenerarTicketAsync(Guid servicioId, Guid sucursalId, Guid? clienteId = null);
        Task<List<Ticket>> ObtenerTicketsEnEsperaAsync(Guid sucursalId);
        Task<Ticket?> ObtenerTicketActualPorVentanillaAsync(Guid ventanillaId);
    }

    public class TicketService : ITicketService
    {
        private readonly ProyectoDBContext _context;

        public TicketService(ProyectoDBContext context)
        {
            _context = context;
        }

        public async Task<Ticket> GenerarTicketAsync(Guid servicioId, Guid sucursalId, Guid? clienteId = null)
        {
            var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.ServicioId == servicioId && s.Estado == "Activo" && !s.Eliminado);
            if (servicio == null)
                throw new Exception("No se puede generar el ticket porque no hay servicios activos o válidos.");

            var sucursal = await _context.Sucursales.FirstOrDefaultAsync(s => s.SucursalId == sucursalId && !s.Eliminado);
            if (sucursal == null)
                throw new Exception("No se encontró una sucursal válida para generar el ticket.");

            // Resolve the client — use the provided account or fall back to the guest record
            Cliente cliente = null;
            if (clienteId.HasValue)
                cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == clienteId.Value && !c.Eliminado);

            if (cliente == null)
            {
                cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.DNI == "00000000" && !c.Eliminado);
                if (cliente == null)
                {
                    cliente = new Cliente
                    {
                        ClienteId        = Guid.NewGuid(),
                        DNI              = "00000000",
                        Nombre_Cliente   = "Invitado",
                        Apellido_Cliente = "Invitado",
                        Estado           = "Activo",
                        TipoCliente      = "Normal"
                    };
                    _context.Clientes.Add(cliente);
                }
            }

            // Resolve priority from the client's TipoCliente (Normal / Preferencial / VIP)
            var tipoPrioridad = cliente.TipoCliente ?? "Normal";
            var prioridad = await _context.Prioridades.FirstOrDefaultAsync(p => p.Nombre == tipoPrioridad && !p.Eliminado);
            if (prioridad == null)
            {
                // Seed values may be missing — create a sensible default keyed to the name
                var pesoMap = new Dictionary<string, int>
                {
                    { "Normal",       1 },
                    { "Preferencial", 2 },
                    { "VIP",          3 }
                };
                prioridad = new Prioridad
                {
                    PrioridadId = Guid.NewGuid(),
                    Nombre      = tipoPrioridad,
                    Descripcion = tipoPrioridad,
                    Peso        = pesoMap.TryGetValue(tipoPrioridad, out var p) ? p : 1
                };
                _context.Prioridades.Add(prioridad);
            }

            // Create Cola instance for this ticket interaction
            var cola = new Cola
            {
                ColaId = Guid.NewGuid(),
                Estado = "En Espera",
                PrioridadId = prioridad.PrioridadId,
                ClienteId = cliente.ClienteId,
                Prioridad = prioridad,
                Cliente = cliente
            };
            _context.Colas.Add(cola);

            // Ticket number: global max for this prefix across all sucursales
            // (unique index on Numero_Ticket is table-wide, not per sucursal)
            var maxPosicion = await _context.Tickets
                .Where(t => t.Numero_Ticket.StartsWith(servicio.Prefijo_Ticket))
                .MaxAsync(t => (int?)t.Posicion) ?? 0;

            int nuevaPosicion = maxPosicion + 1;
            string numeroTicket = $"{servicio.Prefijo_Ticket}{nuevaPosicion:D3}";

            var ticket = new Ticket
            {
                TicketId = Guid.NewGuid(),
                Numero_Ticket = numeroTicket,
                Hora_Emision = DateTime.UtcNow,
                Estado_Ticket = "En espera",
                Posicion = nuevaPosicion,
                ColaId = cola.ColaId,
                ServicioId = servicioId,
                SucursalId = sucursalId,
                Cola = cola,
                Servicio = servicio,
                Sucursal = sucursal
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<List<Ticket>> ObtenerTicketsEnEsperaAsync(Guid sucursalId)
        {
            return await _context.Tickets
                .Include(t => t.Servicio)
                .Include(t => t.Cola).ThenInclude(c => c.Prioridad)
                .Where(t => t.SucursalId == sucursalId && t.Estado_Ticket == "En espera" && !t.Eliminado)
                .OrderByDescending(t => t.Cola.Prioridad.Peso) // Higher weight first
                .ThenBy(t => t.Hora_Emision) // FIFO
                .ToListAsync();
        }

        public async Task<Ticket?> ObtenerTicketActualPorVentanillaAsync(Guid ventanillaId)
        {
            return await _context.Tickets
                .Include(t => t.Servicio)
                .Where(t => t.VentanillaId == ventanillaId && t.Estado_Ticket == "En atención" && !t.Eliminado)
                .FirstOrDefaultAsync();
        }
    }
}
