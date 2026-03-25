namespace Proyecto.Data.Entidades
{
    public class Empleado : EntidadBase
    {
        public Guid EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cargo { get; set; }
        public Guid UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<AsignacionVentanilla> AsignacionVentanillas { get; set; }

        public Empleado()
        {
            AsignacionVentanillas = new HashSet<AsignacionVentanilla>();
        }
    }
}
