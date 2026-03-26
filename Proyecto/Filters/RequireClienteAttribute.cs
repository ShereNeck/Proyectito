using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Proyecto.Filters
{

	public class RequireClienteAttribute : TypeFilterAttribute
	{
		public RequireClienteAttribute() : base(typeof(RequireClienteFilter)) { }

		private class RequireClienteFilter : IAuthorizationFilter
		{
			public void OnAuthorization(AuthorizationFilterContext context)
			{
				var clienteNombre = context.HttpContext.Session.GetString("ClienteNombre");

				if (string.IsNullOrWhiteSpace(clienteNombre))
				{
					context.Result = new RedirectToActionResult("Login", "Cliente", null);
				}
			}
		}
	}
}
