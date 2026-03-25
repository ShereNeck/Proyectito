namespace Proyecto.Data.Entidades
{
    public class Modulo : EntidadBase
    {
        public Guid ModuloId { get; set; }
        public string Nombre { get; set; }
        public string Controller { get; set; }
        public string Metodo { get; set; }

        public Guid ModulosAgrupadoId { get; set; }
        public ModulosAgrupados ModulosAgrupados { get; set; }
        public ICollection<ModulosRoles> ModulosRoles { get; set; }

        public Modulo()
        {
            ModulosRoles = new HashSet<ModulosRoles>();
        }
    }
}
