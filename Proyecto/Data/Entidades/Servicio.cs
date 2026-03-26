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

        public static Servicio Crear(string nombre, string prefijo, string descripcion,
                                     int tiempoEstimado, string estado, Guid userId) => new()
        {
            ServicioId      = Guid.NewGuid(),
            Nombre_Servicio = nombre,
            Prefijo_Ticket  = prefijo,
            Descripcion     = descripcion,
            Tiempo_Estimado = tiempoEstimado,
            Estado          = estado,
            CreateBy        = userId,
            ModifiedBy      = userId
        };

        public void Actualizar(string nombre, string prefijo, string descripcion,
                               int tiempoEstimado, string estado, Guid userId)
        {
            Nombre_Servicio = nombre;
            Prefijo_Ticket  = prefijo;
            Descripcion     = descripcion;
            Tiempo_Estimado = tiempoEstimado;
            Estado          = estado;
            ModifiedBy      = userId;
        }

        public void EliminarLogico(Guid userId) { Eliminado = true; ModifiedBy = userId; }
    }
}
