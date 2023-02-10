using _08_Net_Core_LinqToSql.Models;
using _08_Net_Core_LinqToSql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _08_Net_Core_LinqToSql.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;

        public EnfermosController()
        {
            this.repo = new RepositoryEnfermos();
        }

        public IActionResult Index()
        {
            List<Enfermo> enfermos = this.repo.GetEnfermos();
            return View(enfermos);
        }

        [HttpPost]
        public IActionResult Index(DateTime fechNacimiento)
        {
            List<Enfermo> enfermos = this.repo.FindFechaNacimiento(fechNacimiento);

            if(enfermos != null)
            {
                return View(enfermos);
            }
            else
            {
                ViewData["MENSAJEFECHA"] = "No hay enfermos en la fecha: " + fechNacimiento;
                return View();
            }
        }

        public IActionResult Delete(string inscripcion)
        {
            this.repo.DeleteEnfermo(inscripcion);

            return RedirectToAction("Index");
        }
    }
}
