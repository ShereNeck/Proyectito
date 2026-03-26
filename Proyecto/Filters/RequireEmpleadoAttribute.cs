using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Proyecto.Filters
{
	/// <summary>
	/// Protege rutas que solo pueden acceder empleados (Admin / Agente).
	/// Verifica que el usuario tenga cookie de autenticación activa.
	/// Uso: [RequireEmpleado] o [RequireEmpleado("Administrador")]
	/// </summary>
	public class RequireEmpleadoAttribute : TypeFilterAttribute
	{
		public RequireEmpleadoAttribute(string? rol = null)
			: base(typeof(RequireEmpleadoFilter))
		{
			Arguments = new object[] { rol ?? string.Empty };
		}

		private class RequireEmpleadoFilter : IAuthorizationFilter
		{
			private readonly string _rol;

			public RequireEmpleadoFilter(string rol)
			{
				_rol = rol;
			}

			public void OnAuthorization(AuthorizationFilterContext context)
			{
				var user = context.HttpContext.User;

				if (user?.Identity?.IsAuthenticated != true)
				{
					context.Result = new RedirectToActionResult("Index", "Login", null);
					return;
				}

				if (!string.IsNullOrEmpty(_rol))
				{
					var rolActual = user.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
					if (!rolActual.Equals(_rol, StringComparison.OrdinalIgnoreCase))
					{
						context.Result = new RedirectToActionResult("AccesoDenegado", "Account", null);
					}
				}
			}
		}
	}
}
