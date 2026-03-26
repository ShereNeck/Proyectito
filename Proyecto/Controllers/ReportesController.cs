using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("Admin/[controller]")]
    public class ReportesController : Controller
    {
        private readonly ProyectoDBContext _context;

        public ReportesController(ProyectoDBContext context) => _context = context;

        [HttpGet("")]
        public async Task<IActionResult> Index(DateTime? fecha)
        {
            var targetDate = fecha ?? DateTime.UtcNow.Date;

            var tickets = await _context.Tickets
                .Include(t => t.Servicio)
                .Where(t => t.Hora_Emision.Date == targetDate.Date && !t.Eliminado)
                .ToListAsync();

            var vm = new ReporteDiarioVm
            {
                Fecha         = targetDate,
                Total         = tickets.Count,
                Atendidos     = tickets.Count(t => t.Estado_Ticket == "Finalizado"),
                Ausentes      = tickets.Count(t => t.Estado_Ticket == "Ausente"),
                TiempoPromedio = Math.Round(
                    tickets.Where(t => t.Hora_Atencion.HasValue)
                           .Select(t => (t.Hora_Atencion!.Value - t.Hora_Emision).TotalMinutes)
                           .DefaultIfEmpty(0).Average(), 1),
                Efectividad   = tickets.Count > 0
                    ? Math.Round(tickets.Count(t => t.Estado_Ticket == "Finalizado") * 100.0 / tickets.Count, 1)
                    : 0,
                PorServicio   = tickets
                    .GroupBy(t => t.Servicio?.Nombre_Servicio ?? "Desconocido")
                    .Select(g => new ServicioEstadisticaVm
                    {
                        Nombre         = g.Key,
                        Total          = g.Count(),
                        TiempoPromedio = Math.Round(
                            g.Where(x => x.Hora_Atencion.HasValue)
                             .Select(x => (x.Hora_Atencion!.Value - x.Hora_Emision).TotalMinutes)
                             .DefaultIfEmpty(0).Average(), 1),
                        Efectividad    = g.Count() > 0
                            ? Math.Round(g.Count(x => x.Estado_Ticket == "Finalizado") * 100.0 / g.Count(), 1)
                            : 0
                    }).ToList()
            };


            ViewBag.TargetDate     = targetDate.ToString("yyyy-MM-dd");
            ViewBag.Total          = vm.Total;
            ViewBag.Atendidos      = vm.Atendidos;
            ViewBag.Ausentes       = vm.Ausentes;
            ViewBag.TiempoPromedio = vm.TiempoPromedio;
            ViewBag.Efectividad    = vm.Efectividad;
            ViewBag.ServiceStats   = vm.PorServicio
                .Select(s => new { Name = s.Nombre, Count = s.Total, AvgWait = s.TiempoPromedio, Eff = s.Efectividad })
                .ToList();

            return View("~/Views/Admin/Reportes.cshtml", tickets);
        }
    }
}
