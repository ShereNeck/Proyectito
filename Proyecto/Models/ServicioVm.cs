using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class ServicioVm
    {
        public Guid? ServicioId { get; set; }

        [Required(ErrorMessage = "El nombre del servicio es obligatorio")]
        [Display(Name = "Nombre del servicio")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El prefijo es obligatorio")]
        [Display(Name = "Prefijo del ticket")]
        [StringLength(5, ErrorMessage = "El prefijo no puede superar 5 caracteres")]
        public string Prefijo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tiempo estimado es obligatorio")]
        [Range(1, 120, ErrorMessage = "El tiempo debe estar entre 1 y 120 minutos")]
        [Display(Name = "Tiempo estimado (min)")]
        public int TiempoEstimado { get; set; } = 10;

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";
    }
}
