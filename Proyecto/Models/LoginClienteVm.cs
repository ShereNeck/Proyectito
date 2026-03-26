using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{

	public class LoginClienteVm
	{
		[Required(ErrorMessage = "El usuario es obligatorio")]
		[Display(Name = "Usuario")]
		public string Username { get; set; } = string.Empty;

		[Required(ErrorMessage = "La contraseña es obligatoria")]
		[Display(Name = "Contraseña")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
	}
}
