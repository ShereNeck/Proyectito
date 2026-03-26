using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{

    public class VentanillaVm
    {
        public Guid? VentanillaId { get; set; }

        [Required(ErrorMessage = "El número de ventanilla es obligatorio")]
        [Display(Name = "Número de ventanilla")]
        [StringLength(10)]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Cerrada";

        [Required(ErrorMessage = "Debe seleccionar una sucursal")]
        [Display(Name = "Sucursal")]
        public Guid SucursalId { get; set; }

        public string? SucursalNombre { get; set; }

        public Guid[] ServicioIds { get; set; } = Array.Empty<Guid>();
        public List<string> ServicioNombres { get; set; } = new();

        public Guid? EmpleadoId { get; set; }

        public string? EmpleadoAsignado { get; set; }
    }
}
