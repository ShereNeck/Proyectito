namespace Proyecto.Data.Entidades
{
    public class Rol : EntidadBase
    {
        public Guid RolId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
        public ICollection<ModulosRoles> ModulosRoles { get; set; }

        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
            ModulosRoles = new HashSet<ModulosRoles>();
        }

        public static Rol Crear(string nombre, string descripcion, Guid userId) => new()
        {
            RolId       = Guid.NewGuid(),
            Nombre      = nombre,
            Descripcion = descripcion,
            Estado      = "Activo",
            CreateBy    = userId,
            ModifiedBy  = userId
        };

        public void Actualizar(string nombre, string descripcion, Guid userId)
        {
            Nombre      = nombre;
            Descripcion = descripcion;
            ModifiedBy  = userId;
        }

        public void EliminarLogico(Guid userId) { Eliminado = true; ModifiedBy = userId; }
    }
}
