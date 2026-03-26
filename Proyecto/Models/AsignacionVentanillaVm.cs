using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class AsignacionVentanillaVm
    {
        public Guid? AsignacionId { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        [Display(Name = "Hora de inicio")]
        public DateTime Hora_Inicio { get; set; }

        [Display(Name = "Hora de fin")]
        public DateTime? Hora_Fin { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado")]
        public Guid EmpleadoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una ventanilla")]
        public Guid VentanillaId { get; set; }

        public string NombreEmpleado { get; set; } = string.Empty;
        public string NombreVentanilla { get; set; } = string.Empty;
    }
}
