using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{

    public class RolVm
    {
        public Guid? RolId { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [StringLength(300)]
        public string? Descripcion { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        public int TotalUsuarios { get; set; }
    }
}
