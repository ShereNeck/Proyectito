using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    /// <summary>
    /// ViewModel para crear / editar un Empleado y mostrarlo en la lista.
    /// Los campos de usuario (NombreUsuario, Contrasena, RolId) solo se usan en la creación.
    /// Los campos *Display son de solo lectura para la lista.
    /// </summary>
    public class EmpleadoVm
    {
        public Guid? EmpleadoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Display(Name = "Apellido")]
        [StringLength(150)]
        public string Apellido { get; set; } = string.Empty;

        [Display(Name = "Servicio / Cargo")]
        [StringLength(100)]
        public string? Cargo { get; set; }

        [Display(Name = "Nombre de usuario")]
        [StringLength(150)]
        public string? NombreUsuario { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string? Contrasena { get; set; }

        [Display(Name = "Rol")]
        public Guid? RolId { get; set; }

        public string? NombreRol { get; set; }
        public string? EmailUsuario { get; set; }
        public string? NombreUsuarioDisplay { get; set; }
    }
}
