using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto.Controllers
{


    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        public IActionResult Index()      => RedirectToAction("Index", "Ventanillas");
        public IActionResult Servicios()  => RedirectToAction("Index", "Servicios");
        public IActionResult Sucursales() => RedirectToAction("Index", "Sucursales");
        public IActionResult Empleados()  => RedirectToAction("Index", "Empleados");
        public IActionResult Clientes()   => RedirectToAction("Index", "Clientes");
        public IActionResult Roles()      => RedirectToAction("Index", "Roles");
        public IActionResult Reportes()   => RedirectToAction("Index", "Reportes");
    }
}
