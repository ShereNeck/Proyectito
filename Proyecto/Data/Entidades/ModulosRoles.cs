namespace Proyecto.Data.Entidades
{
    public class ModulosRoles : EntidadBase
    {
        public Guid ModulosRolesId { get; set; }
        public string Descripcion { get; set; }
        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public Guid ModuloId { get; set; }
        public Guid RolId { get; set; }

        public Modulo Modulo { get; set; }
        public Rol Rol { get; set; }
    }
}
