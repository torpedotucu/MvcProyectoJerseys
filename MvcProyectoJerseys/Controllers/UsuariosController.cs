using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;

namespace MvcProyectoJerseys.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryCamisetas repo;
        private HelperPathProvider helper;
        
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
        public async Task<IActionResult> CreateUsuario(string nombre, string alias, IFormFile avatar,string correo, string contrasena, string equipo, string pais)
        {
            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            UsuarioPuro user=new UsuarioPuro();
            user.IdUsuario=await this.repo.GetMaxIdUsuario();
            user.UserName=nombre;
            user.AliasName=alias;
            user.Correo=correo;
            user.Salt=HelperCryptography.GenerateSalt();
            user.Contrasena=HelperCryptography.EncryptPassword(contrasena, user.Salt);
            user.Equipo=equipo;
            user.Pais=pais;
            user.Avatar=avatar.FileName;
            user.FechaUnion=DateTime.Now;
            await this.repo.SubirFichero(avatar, Folders.Avatar);
            await this.repo.CreateUsuario(user);
            return View();
        }

    }
}
