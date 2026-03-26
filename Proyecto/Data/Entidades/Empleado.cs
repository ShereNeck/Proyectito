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

        // Crear se gestiona en el controlador porque también crea un Usuario asociado.

        public void Actualizar(string nombre, string apellido, string cargo, Guid userId)
        {
            Nombre     = nombre;
            Apellido   = apellido;
            Cargo      = cargo;
            ModifiedBy = userId;
        }

        public void EliminarLogico(Guid userId) { Eliminado = true; ModifiedBy = userId; }
    }
}
