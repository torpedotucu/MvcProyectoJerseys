using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;
using MvcProyectoJerseys.Services;
using System.Security.Claims;

namespace MvcProyectoJerseys.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceCamisetas service;
        public ManagedController(ServiceCamisetas serviceCamisetas)
        {
            this.service=service;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(string username,string password)
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<List<Usuario>> GetListaAmigosAsync()
        {
            //SE DEBERIA HACER UNA LLAMADA APARTE PARA OBTENER LA LISTA DE AMIGOS DE UN USUARIO APARTE
            //CAMBIAR LA LLAMADA Y PASARLE POR PARAMETRO IDUSUARIO??
        }
    }
}
