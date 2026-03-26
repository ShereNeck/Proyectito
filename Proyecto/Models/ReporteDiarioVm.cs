namespace Proyecto.Models
{

    public class ReporteDiarioVm
    {
        public DateTime Fecha { get; set; }

        public int Total { get; set; }
        public int Atendidos { get; set; }
        public int Ausentes { get; set; }
        public double TiempoPromedio { get; set; }
        public double Efectividad { get; set; }

        public List<ServicioEstadisticaVm> PorServicio { get; set; } = new();
    }


    public class ServicioEstadisticaVm
    {
        public string Nombre { get; set; } = string.Empty;
        public int Total { get; set; }
        public double TiempoPromedio { get; set; }
        public double Efectividad { get; set; }
    }
}
