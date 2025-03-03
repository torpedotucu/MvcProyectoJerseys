using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Extensions;
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
        [ValidateAntiForgeryToken]
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
            await this.repo.CreateUsuario(user);
            await this.repo.SubirFichero(avatar, Folders.Avatar);
            return View();
        }


        public IActionResult Login()
        {
            HttpContext.Session.Remove("USUARIO");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string password)
        {
            UsuarioPuro user = await this.repo.LoginUsuario(correo, password);
            if (user==null)
            {
                ViewBag.ERROR=true;
                return View();
            }
            else
            {
                HttpContext.Session.SetObject("USUARIO", user);
                ViewBag.INICIO=user.Equipo;
                return RedirectToAction("PerfilUsuario","Camisetas");
            }
        }

    }
}
