using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class PrioridadVm
    {
        public Guid? PrioridadId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El peso es obligatorio")]
        [Range(1, 100, ErrorMessage = "El peso debe estar entre 1 y 100")]
        [Display(Name = "Peso (orden en cola)")]
        public int Peso { get; set; }
    }
}
