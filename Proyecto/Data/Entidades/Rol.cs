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
    }
}
