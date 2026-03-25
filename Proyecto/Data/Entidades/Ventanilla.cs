namespace Proyecto.Data.Entidades
{
    public class Ventanilla : EntidadBase
    {
        public Guid VentanillaId { get; set; }
        public string Numero_Ventanilla { get; set; }
        public string Estado_Ventanilla { get; set; }
        public Guid SucursalId { get; set; }

        public Sucursal Sucursal { get; set; }
        public ICollection<VentanillaServicio> VentanillaServicios { get; set; }
        public ICollection<AsignacionVentanilla> AsignacionVentanillas { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        public Ventanilla()
        {
            VentanillaServicios = new HashSet<VentanillaServicio>();
            AsignacionVentanillas = new HashSet<AsignacionVentanilla>();
            Tickets = new HashSet<Ticket>();
        }
    }
}
