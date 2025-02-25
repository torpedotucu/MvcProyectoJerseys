using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;

namespace MvcProyectoJerseys.Controllers
{
    public class UsuariosController : Controller
    {
        RepositoryCamisetas repo;
        HelperPathProvider helper;
        public UsuariosController(RepositoryCamisetas repo, HelperPathProvider helper)
        {
            this.repo=repo;
            this.helper=helper;
        }

        public async Task<IActionResult> CreateUsuario()
        {
            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUsuario(string nombre, string alias, IFormFile avatar,string correo, string contraseña, string equipo, string pais)
        {
            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            
        }

    }
}
