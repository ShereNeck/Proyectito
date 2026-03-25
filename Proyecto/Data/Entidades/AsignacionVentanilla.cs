namespace Proyecto.Data.Entidades
{
    public class AsignacionVentanilla : EntidadBase
    {
        public Guid AsignacionId { get; set; }
        public DateTime Hora_Inicio { get; set; }
        public DateTime? Hora_Fin { get; set; }
        public Guid EmpleadoId { get; set; }
        public Guid VentanillaId { get; set; }

        public Empleado Empleado { get; set; }
        public Ventanilla Ventanilla { get; set; }
    }
}
