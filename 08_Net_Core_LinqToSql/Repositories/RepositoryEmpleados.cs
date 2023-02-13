using _08_Net_Core_LinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace _08_Net_Core_LinqToSql.Repositories
{
    public class RepositoryEmpleados
    {
        private DataTable tablaEmpleados;

        public RepositoryEmpleados()
        {
            string connectionString = @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2022"; ;

            string consulta = "SELECT * FROM EMP";
            SqlDataAdapter adapter = new SqlDataAdapter(consulta, connectionString);
            this.tablaEmpleados = new DataTable();
            adapter.Fill(this.tablaEmpleados);
        }

        //METODO PARA RECUPERAR TODOS LOS EMPLEADOS
        public List<Empleado> GetEmpleados()
        {
            //LA TABLA ESTA COMPUESTA POR FILAS (DATAROW), LA CONSULTA DEBE SER DOBRE TODAS LAS FILAS DE LA TABLA
            var consulta = from datos in this.tablaEmpleados.AsEnumerable() 
                           select datos;

            //AHORA MISMO TENEMOS EN CONSULTA UNA COLECCION LINQ DE OBJETOS DATAROW QUE PODEMOS ORDENAR, FILTRAR Y TODO LO QUE DESEEMOS
            List<Empleado> empleados = new List<Empleado>();
            //VAMOS A RECORRER TODOS LOS DATOS DE LA CONSULTA Y EXTRAERLOS
            foreach(var row in consulta)
            {
                Empleado empleado = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO")
                };
                empleados.Add(empleado);
            }
            return empleados;
        }

        public Empleado FindEmpleado(int idEmpleado)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("EMP_NO") == idEmpleado
                           select datos;
            //NOSOTROS SABEMOS QUE DEVUELVE SOLAMENTE UNA FILA PERO CONSULTA NO LO SABE
            //CONSULTA CONTIENE UNA SERIE DE METODOS PARA TRATAR LOS DATOS O RECUPERARLOS O REALIZAR CIERTAS ACCIONES
            //TENEMOS UN METODO PARA RECUPERAR LA PRIMERA FILA DE LA CONSULTA .FIRST()
            var row = consulta.First();
            Empleado empleado = new Empleado
            {
                IdEmpleado = row.Field<int>("EMP_NO"),
                Apellido = row.Field<string>("APELLIDO"),
                Oficio = row.Field<string>("OFICIO"),
                Salario = row.Field<int>("SALARIO"),
                IdDepartamento = row.Field<int>("DEPT_NO")
            };
            return empleado;
        }

        public List<Empleado> GetEmpleados(string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;

            if(consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Empleado> empleados = new List<Empleado>();

                foreach(var row in consulta)
                {
                    Empleado empleado = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };
                    empleados.Add(empleado);
                }
                return empleados;
            }
        }

        //METODO PARA DEVOLVER EL RESUMEN DE EMPLEADOS POR OFICIO
        public ResumenEmpleados GetEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;
            consulta = consulta.OrderBy( x => x.Field<int>("SALARIO") );

            int personas = consulta.Count();
            int maximo = consulta.Max( z => z.Field<int>("SALARIO") );
            double media = consulta.Average(j => j.Field<int>("SALARIO") );

            List<Empleado> empleados = new List<Empleado>();
            foreach(var row in consulta)
            {
                Empleado empleado = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO")
                };
                empleados.Add(empleado);
            }

            ResumenEmpleados model = new ResumenEmpleados
            {
                Empleados = empleados,
                MaximoSalario = maximo,
                Personas = personas,
                MediaSalarial = media
            };

            return model;
        }

        //METODO PARA DEOLVER TODOS LOS OFICIOS LIST<STRING>
        public List<string> GetOficios()
        {
            var consulta = (from datos in this.tablaEmpleados.AsEnumerable()
                               //select new { Oficio = datos.Field<string>("OFICIO") };
                           select datos.Field<string>("OFICIO")).Distinct();

            List<string> oficios = new List<string>();

            foreach(var oficio in consulta)
            {
                oficios.Add(oficio);
            }
            return oficios;
        }
    }
}
