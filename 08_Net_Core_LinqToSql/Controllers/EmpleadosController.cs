using _08_Net_Core_LinqToSql.Models;
using _08_Net_Core_LinqToSql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _08_Net_Core_LinqToSql.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController()
        {
            this.repo = new RepositoryEmpleados();
        }

        public IActionResult Index()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();

            return View(empleados);
        }
        public IActionResult Details(int idEmpleado)
        {
            Empleado empleado = this.repo.FindEmpleado(idEmpleado);

            return View(empleado);
        }

        public IActionResult BuscadorEmpleados()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();

            return View(empleados);
        }

        [HttpPost]
        public IActionResult BuscadorEmpleados(string oficio, int salario)
        {
            List<Empleado> empleados = this.repo.GetEmpleados(oficio, salario);

            if(empleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados con oficio " + oficio + " o salario superior a " + salario;
                return View();
            }
            else
            {
                return View(empleados);
            }

        }

        public IActionResult DatosEmpleados()
        {
            List<string> oficios = this.repo.GetOficios();
            ViewData["OFICIOS"] = oficios;
            return View();
        }

        [HttpPost]
        public IActionResult DatosEmpleados(string oficio)
        {
            List<string> oficios = this.repo.GetOficios();
            ViewData["OFICIOS"] = oficios;
            ResumenEmpleados model = this.repo.GetEmpleadosOficio(oficio);

            return View(model);
        }
    }
}
