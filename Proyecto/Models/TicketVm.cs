using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class TicketVm
    {
        public Guid TicketId { get; set; }

        [Display(Name = "Número de ticket")]
        public string Numero_Ticket { get; set; } = string.Empty;

        [Display(Name = "Hora de emisión")]
        public DateTime Hora_Emision { get; set; }

        [Display(Name = "Hora de atención")]
        public DateTime? Hora_Atencion { get; set; }

        [Display(Name = "Hora de finalización")]
        public DateTime? Hora_Finalizacion { get; set; }

        [Display(Name = "Estado")]
        public string Estado_Ticket { get; set; } = string.Empty;

        [Display(Name = "Posición")]
        public int Posicion { get; set; }

        public Guid ColaId { get; set; }
        public Guid ServicioId { get; set; }
        public Guid SucursalId { get; set; }
        public Guid? VentanillaId { get; set; }


        public string NombreServicio { get; set; } = string.Empty;
        public string? NombreVentanilla { get; set; }
        public string? PrioridadNombre { get; set; }
        public int PrioridadPeso { get; set; }
    }
}
