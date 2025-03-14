using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Extensions;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;
using System.Security.Claims;

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
            int idUser=await this.repo.GetMaxIdUsuario();
            user.IdUsuario=idUser;
            user.UserName=nombre;
            user.AliasName=alias;
            user.Correo=correo;
            user.Salt=HelperCryptography.GenerateSalt();
            user.Contrasena=HelperCryptography.EncryptPassword(contrasena, user.Salt);
            user.Equipo=equipo;
            user.CodeAmistad=this.repo.GenerateCodeAmistadUsuario();
            user.Pais=pais;
            string filename = this.repo.GenerateUniqueFileName(idUser, avatar);
            user.Avatar=filename;
            user.FechaUnion=DateTime.Now;
            await this.repo.CreateUsuario(user);
            await this.repo.SubirFichero(avatar, Folders.Avatar, filename);
            return RedirectToAction("Login");
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
            Usuario user = await this.repo.LoginUsuario(correo, password);
            if (user==null)
            {
                ViewBag.ERROR=true;
                return View();
            }
            else
            {
                //HttpContext.Session.SetObject("USUARIO", user);
                //ViewBag.INICIO=user.Equipo;
                //return RedirectToAction("PerfilUsuario","Camisetas");
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                Claim claimUserName = new Claim(ClaimTypes.Name, user.UserName);
                Claim claimIdUser = new Claim("IDUSUARIO", user.IdUsuario.ToString());
                identity.AddClaim(claimUserName);
                identity.AddClaim(claimIdUser);
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                    {
                        ExpiresUtc=DateTime.Now.AddMinutes(30)
                    });

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                if (TempData["id"]!=null)
                {
                    string id = TempData["id"].ToString();
                    return RedirectToAction(action, controller, new { id = id });
                }
                else
                {
                    return RedirectToAction(action, controller);
                }
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
