using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MvcProyectoJerseys.Extensions;
using MvcProyectoJerseys.Helpers;
//using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;
using MvcProyectoJerseys.Services;
using NuGet.Common;
using NugetJerseyHubRGO.Models;
using System.Security.Claims;

namespace MvcProyectoJerseys.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryCamisetas repo;
        private ServiceCamisetas service;
        private HelperPathProvider helper;
        
        public UsuariosController(RepositoryCamisetas repo, HelperPathProvider helper,ServiceCamisetas service)
        {
            this.repo=repo;
            this.helper=helper;
            this.service=service;
            
        }

        public async Task<IActionResult> CreateUsuario()
        {
            List<Pais> paises = await this.service.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateUsuario(string nombre, string alias, IFormFile avatar,string correo, string contrasena, string equipo, string pais)
        {
            //if (await this.service.ExisteCorreo(correo))
            //{
            //    ModelState.AddModelError("Email", "El correo ya está registrado.");
            //    return View();
            //}
            List<Pais> paises = await this.service.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            //UsuarioPuro user=new UsuarioPuro();
            //int idUser=await this.repo.GetMaxIdUsuario();
            //user.IdUsuario=idUser;
            //user.UserName=nombre;
            //user.AliasName=alias;
            //user.Correo=correo;
            //user.Salt=HelperCryptography.GenerateSalt();
            //user.Contrasena=HelperCryptography.EncryptPassword(contrasena, user.Salt);
            //user.Equipo=equipo;
            //user.CodeAmistad=this.repo.GenerateCodeAmistadUsuario();
            //user.Pais=pais;

            string filename = this.repo.GenerateUniqueFileName(23, avatar);
            UsuarioCreateDTO usuarioCreate = new UsuarioCreateDTO
            {
                UserName=nombre,
                AliasName=alias,
                Correo=correo,
                Contrasena=contrasena,
                Avatar=filename,
                Equipo=equipo,
                Pais=pais
            };

            
            //user.Avatar=filename;
            //user.FechaUnion=DateTime.Now;
            int id=await this.service.CreateUsuario(usuarioCreate);
            
            await this.repo.SubirFichero(avatar, Folders.Avatar, filename);
            return RedirectToAction("Login");
        }


        public IActionResult Login()
        {
            HttpContext.Session.Remove("USUARIO");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(string correo, string password)
        //{
        //    Usuario user = await this.repo.LoginUsuario(correo, password);
        //    if (user==null)
        //    {
        //        ViewBag.ERROR=true;
        //        return View();
        //    }
        //    else
        //    {

        //        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
        //        Claim claimUserName = new Claim(ClaimTypes.Name, user.UserName);
        //        Claim claimIdUser = new Claim("IDUSUARIO", user.IdUsuario.ToString());
        //        identity.AddClaim(claimUserName);
        //        identity.AddClaim(claimIdUser);
        //        ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
        //        await HttpContext.SignInAsync(
        //            CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
        //            {
        //                ExpiresUtc=DateTime.Now.AddMinutes(30)
        //            });

        //        string controller = TempData["controller"].ToString();
        //        string action = TempData["action"].ToString();
        //        if (TempData["id"]!=null)
        //        {
        //            string id = TempData["id"].ToString();
        //            return RedirectToAction(action, controller, new { id = id });
        //        }
        //        else
        //        {
        //            return RedirectToAction(action, controller);
        //        }
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string password)
        {
            LoginModel loginModel = new LoginModel();
            loginModel.UserName=correo;
            loginModel.Password=password;
            string token = await this.service.GetTokenAsync(loginModel.UserName, loginModel.Password);
            if (token==null)
            {
                ViewBag.ERROR=true;
                ViewData["MENSAJE"]="Usuario/Password Incorrectos";
                return View();
            }
            else
            {
                ViewData["MENSAJE"] = "Ya tienes tu Token!!!";
                HttpContext.Session.SetString("TOKEN", token);
                
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim
                    (ClaimTypes.Name, loginModel.UserName));

                //GUARDAR EL ID EN VEZ DE LA CONTRASEÑA
                //Usuario usuario=this.service.GetUsuarioCorreo()
                //identity.AddClaim(new Claim
                //    (ClaimTypes.NameIdentifier, loginModel.Password));


                identity.AddClaim(new Claim("TOKEN", token));
                ClaimsPrincipal principal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , principal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });
                Usuario usuario = await this.service.GetUsuarioCorreo(correo); // <-- este método deberías crearlo si no existe
                if (usuario != null)
                {
                    // 4. Guardar el objeto Usuario en sesión
                    HttpContext.Session.SetObject("USUARIO", usuario);
                }
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
