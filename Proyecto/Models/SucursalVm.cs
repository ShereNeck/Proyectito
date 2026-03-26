using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{

    public class SucursalVm
    {
        public Guid? SucursalId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [Display(Name = "Dirección")]
        [StringLength(255)]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Ingrese un teléfono válido")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activa";
    }
}
