namespace Proyecto.Data.Entidades
{
    public class ModulosAgrupados : EntidadBase
    {
        public Guid ModulosAgrupadosId { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Modulo> Modulos { get; set; }

        public ModulosAgrupados()
        {
            Modulos = new HashSet<Modulo>();
        }
    }
}
