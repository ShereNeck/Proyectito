using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class ClienteVm
    {
        public Guid? ClienteId { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [Display(Name = "DNI")]
        [StringLength(20)]
        public string DNI { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(150)]
        public string Nombre_Cliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Display(Name = "Apellido")]
        [StringLength(150)]
        public string Apellido_Cliente { get; set; } = string.Empty;

        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? Fecha_Nacimiento { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [Display(Name = "Tipo de cliente")]
        public string TipoCliente { get; set; } = "Normal";
    }
}
