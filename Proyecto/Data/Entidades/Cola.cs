namespace Proyecto.Data.Entidades
{
    public class Cola : EntidadBase
    {
        public Guid ColaId { get; set; }
        public string Estado { get; set; }
        public Guid PrioridadId { get; set; }
        public Guid ClienteId { get; set; }

        public Prioridad Prioridad { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        public Cola()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}
