namespace Proyecto.Data.Entidades
{
    public class Usuario : EntidadBase
    {
        public Guid UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Guid RolId { get; set; }

        public Rol Rol { get; set; }
        public Empleado Empleado { get; set; }
        public Cliente Cliente { get; set; }

        public void EliminarLogico(Guid userId) { Eliminado = true; ModifiedBy = userId; }
    }
}
