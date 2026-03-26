namespace Proyecto.Data.Entidades
{
    public abstract class EntidadBase
    {
        public Guid? CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Eliminado { get; set; }
    }
}
