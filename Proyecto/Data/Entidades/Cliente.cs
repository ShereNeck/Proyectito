namespace Proyecto.Data.Entidades
{
    public class Cliente : EntidadBase
    {
        public Guid ClienteId { get; set; }
        public string DNI { get; set; }
        public string Nombre_Cliente { get; set; }
        public string Apellido_Cliente { get; set; }
        public DateTime? Fecha_Nacimiento { get; set; }
        public string Estado { get; set; }
        public string TipoCliente { get; set; } = "Normal";
        public Guid? UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<Cola> Colas { get; set; }

        public Cliente()
        {
            Colas = new HashSet<Cola>();
        }
    }
}
