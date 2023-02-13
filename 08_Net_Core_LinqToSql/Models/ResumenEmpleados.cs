namespace _08_Net_Core_LinqToSql.Models
{
    public class ResumenEmpleados
    {
        public int Personas { get; set; }
        public int MaximoSalario { get; set; }
        public double MediaSalarial { get; set; }
        public List<Empleado> Empleados { get; set; }
    }
}
