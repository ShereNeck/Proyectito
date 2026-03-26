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

        public static Sucursal Crear(string nombre, string direccion, string telefono,
                                     string estado, Guid userId) => new()
        {
            SucursalId = Guid.NewGuid(),
            Nombre     = nombre,
            Direccion  = direccion,
            Telefono   = telefono,
            Estado     = estado,
            CreateBy   = userId,
            ModifiedBy = userId
        };

        public void Actualizar(string nombre, string direccion, string telefono,
                               string estado, Guid userId)
        {
            Nombre     = nombre;
            Direccion  = direccion;
            Telefono   = telefono;
            Estado     = estado;
            ModifiedBy = userId;
        }

        public void EliminarLogico(Guid userId) { Eliminado = true; ModifiedBy = userId; }
    }
}
