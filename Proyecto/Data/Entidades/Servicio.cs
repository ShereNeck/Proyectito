namespace Proyecto.Data.Entidades
{
    public class Servicio : EntidadBase
    {
        public Guid ServicioId { get; set; }
        public string Nombre_Servicio { get; set; }
        public string Prefijo_Ticket { get; set; }
        public string Descripcion { get; set; }
        public int Tiempo_Estimado { get; set; }
        public string Estado { get; set; }

        public ICollection<VentanillaServicio> VentanillaServicios { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        public Servicio()
        {
            VentanillaServicios = new HashSet<VentanillaServicio>();
            Tickets = new HashSet<Ticket>();
        }
    }
}
