namespace Proyecto.Data.Entidades
{
    public class Ticket : EntidadBase
    {
        public Guid TicketId { get; set; }
        public string Numero_Ticket { get; set; }
        public DateTime Hora_Emision { get; set; }
        public DateTime? Hora_Atencion { get; set; }
        public DateTime? Hora_Finalizacion { get; set; }
        public string Estado_Ticket { get; set; }
        public int Posicion { get; set; }
        public Guid ColaId { get; set; }
        public Guid ServicioId { get; set; }
        public Guid SucursalId { get; set; }
        public Guid? VentanillaId { get; set; }

        public Cola Cola { get; set; }
        public Servicio Servicio { get; set; }
        public Sucursal Sucursal { get; set; }
        public Ventanilla Ventanilla { get; set; }
    }
}
