namespace Proyecto.Data.Entidades
{
    public class Prioridad : EntidadBase
    {
        public Guid PrioridadId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Peso { get; set; }

        public ICollection<Cola> Colas { get; set; }

        public Prioridad()
        {
            Colas = new HashSet<Cola>();
        }
    }
}
