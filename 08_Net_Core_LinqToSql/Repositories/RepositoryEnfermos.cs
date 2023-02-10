using _08_Net_Core_LinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace _08_Net_Core_LinqToSql.Repositories
{
    public class RepositoryEnfermos
    {
        private DataTable tablaEnfermos;
        private SqlCommand command;
        private SqlConnection connection;
        private SqlDataReader reader;

        public RepositoryEnfermos()
        {
            string connectionString = @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2022";

            string connectionStringCasa = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2022";

            string consulta = "SELECT * FROM ENFERMO";
            SqlDataAdapter adapter = new SqlDataAdapter(consulta, connectionStringCasa);
            this.tablaEnfermos = new DataTable();
            adapter.Fill(tablaEnfermos);

            this.connection = new SqlConnection(connectionStringCasa);
            this.command = new SqlCommand();
            this.command.Connection = this.connection;
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           select datos;

            List<Enfermo> enfermos = new List<Enfermo>();

            foreach(var row in consulta)
            {
                Enfermo enfermo = new Enfermo
                {
                    Inscripcion = row.Field<string>("INSCRIPCION"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Direccion = row.Field<string>("DIRECCION"),
                    FechNacimiento = row.Field<DateTime>("FECHA_NAC"),
                    Sexo = row.Field<string>("S"),
                    NumSS = row.Field<string>("NSS")
                };
                enfermos.Add(enfermo);
            }
            return enfermos;
        }

        public void DeleteEnfermo(string inscripcion)
        {
            string consulta = "DELETE FROM ENFERMO WHERE INSCRIPCION = @INSCRIPCION";

            SqlParameter paramFechaNacimiento = new SqlParameter("@INSCRIPCION", inscripcion);
            this.command.Parameters.Add(paramFechaNacimiento);

            this.command.CommandType = CommandType.Text;
            this.command.CommandText = consulta;

            this.connection.Open();
            this.command.ExecuteNonQuery();

            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public List<Enfermo> FindFechaNacimiento(DateTime fechNacimiento)
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                              where datos.Field<DateTime>("FECHA_NAC") >= fechNacimiento
                           select datos;

            List<Enfermo> enfermos = new List<Enfermo>();

            foreach (var row in consulta)
            {
                Enfermo enfermo = new Enfermo
                {
                    Inscripcion = row.Field<string>("INSCRIPCION"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Direccion = row.Field<string>("DIRECCION"),
                    FechNacimiento = row.Field<DateTime>("FECHA_NAC"),
                    Sexo = row.Field<string>("S"),
                    NumSS = row.Field<string>("NSS")
                };
                enfermos.Add(enfermo);
            }
            return enfermos;
        }
    }
}
