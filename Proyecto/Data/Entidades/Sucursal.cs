namespace Proyecto.Data.Entidades
{
    public class Sucursal : EntidadBase
    {
        public Guid SucursalId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; }

        public ICollection<Ventanilla> Ventanillas { get; set; }

        public Sucursal()
        {
            Ventanillas = new HashSet<Ventanilla>();
        }
    }
}
