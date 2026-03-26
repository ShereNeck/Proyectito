using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{

	public class RegistroClienteVm
	{
		[Required(ErrorMessage = "El nombre es obligatorio")]
		[Display(Name = "Nombre")]
		public string Nombre { get; set; } = string.Empty;

		[Required(ErrorMessage = "El apellido es obligatorio")]
		[Display(Name = "Apellido")]
		public string Apellido { get; set; } = string.Empty;

		[Required(ErrorMessage = "El DNI es obligatorio")]
		[Display(Name = "DNI")]
		[StringLength(13, MinimumLength = 13, ErrorMessage = "El DNI debe tener exactamente 13 dígitos")]
		[RegularExpression(@"^\d{13}$", ErrorMessage = "El DNI debe contener solo dígitos")]
		public string Dni { get; set; } = string.Empty;

		[EmailAddress(ErrorMessage = "Ingrese un correo válido")]
		[Display(Name = "Correo electrónico")]
		public string? Email { get; set; }

		[Display(Name = "Fecha de nacimiento")]
		[DataType(DataType.Date)]
		public DateTime? FechaNacimiento { get; set; }

		[Required(ErrorMessage = "El usuario es obligatorio")]
		[Display(Name = "Nombre de usuario")]
		public string Username { get; set; } = string.Empty;

		[Required(ErrorMessage = "La contraseña es obligatoria")]
		[Display(Name = "Contraseña")]
		[DataType(DataType.Password)]
		[MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe confirmar la contraseña")]
		[Display(Name = "Confirmar contraseña")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
		public string Confirmar { get; set; } = string.Empty;
	}
}
