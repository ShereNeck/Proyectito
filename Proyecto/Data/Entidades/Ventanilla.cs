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

        public static Ventanilla Crear(string numero, string estado, Guid sucursalId,
                                       Guid userId) => new()
        {
            VentanillaId      = Guid.NewGuid(),
            Numero_Ventanilla = numero,
            Estado_Ventanilla = estado,
            SucursalId        = sucursalId,
            CreateBy          = userId,
            ModifiedBy        = userId
        };

        public void Actualizar(string numero, string estado, Guid sucursalId, Guid userId)
        {
            Numero_Ventanilla = numero;
            Estado_Ventanilla = estado;
            SucursalId        = sucursalId;
            ModifiedBy        = userId;
        }

        public void EliminarLogico(Guid userId) { Eliminado = true; ModifiedBy = userId; }
    }
}
