using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class ColaVm
    {
        public Guid ColaId { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;

        public Guid PrioridadId { get; set; }
        public Guid ClienteId { get; set; }

        public string NombrePrioridad { get; set; } = string.Empty;
        public int PesoPrioridad { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
    }
}
