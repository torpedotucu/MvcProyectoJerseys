using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;

namespace MvcProyectoJerseys.Controllers
{
    public class CamisetasController : Controller
    {
        RepositoryCamisetas repo;
        public CamisetasController(RepositoryCamisetas repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PerfilUsuario(int idUsuario)
        {
            Usuario usuario = this.repo.GetUsuario(idUsuario);
            ViewData["USUARIO"] = usuario;
            Console.WriteLine(usuario.UserName);
            List<Camiseta> camisetas = this.repo.GetCamisetasUsuario(idUsuario);
            ViewData["CAMISETAS"] = camisetas;
            return View(usuario);
        }
        public IActionResult DetalleCamiseta(int idCamiseta)
        {
            Camiseta camiseta = this.repo.GetCamiseta(idCamiseta);
            return View(camiseta);
        }
        public IActionResult UpdateCamiseta(int idCamiseta, string equipo, string pais, int year, string marca, string equipacion, string descripcion, int posicion, string condicion, int dorsal, string jugador, int esActiva, string imagen)
        {
            Camiseta camiseta = this.repo.GetCamiseta(idCamiseta);
            camiseta.Equipo = equipo;
            camiseta.Pais = pais;
            camiseta.Year = year;
            camiseta.Marca = marca;
            camiseta.Equipacion = equipacion;
            camiseta.Descripcion = descripcion;
            camiseta.Posicion = posicion;
            camiseta.Condicion = condicion;
            camiseta.Dorsal = dorsal;
            camiseta.Jugador = jugador;
            camiseta.EsActiva = esActiva;
            camiseta.FechaSubida = DateTime.Now;
            camiseta.Imagen = imagen;
            this.repo.ModificarCamiseta(camiseta);
            return RedirectToAction("PerfilUsuario", new { idUsuario = camiseta.IdUsuario });
        }
        
        
    }
}
