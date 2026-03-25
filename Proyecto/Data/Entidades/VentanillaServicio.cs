namespace Proyecto.Data.Entidades
{
    public class VentanillaServicio : EntidadBase
    {
        public Guid VentanillaServicioId { get; set; }
        public Guid VentanillaId { get; set; }
        public Guid ServicioId { get; set; }
        public bool Activo { get; set; }

        public Ventanilla Ventanilla { get; set; }
        public Servicio Servicio { get; set; }
    }
}
