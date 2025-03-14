using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MvcProyectoJerseys.Extensions;
using MvcProyectoJerseys.Filters;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MvcProyectoJerseys.Controllers
{
    public class CamisetasController : Controller
    {
        RepositoryCamisetas repo;
        HelperPathProvider helper;
        public CamisetasController(RepositoryCamisetas repo, HelperPathProvider helper)
        {
            this.repo = repo;
            this.helper=helper;
        }
        [AuthorizeUsers]
        public async Task<IActionResult> Index()
        {
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            int idUsuario = int.Parse(HttpContext.User.FindFirst("IDUSUARIO").Value);
            List<Camiseta> camisetas = await this.repo.GetPublicacionesInicio(idUsuario);
            return View(camisetas);
        }
        [AuthorizeUsers]
        public async Task<IActionResult> PerfilUsuario()
        {
            //Usuario usuario=HttpContext.Session.GetObject<Usuario>("USUARIO");
            int dato =int.Parse( HttpContext.User.FindFirst("IDUSUARIO").Value);
            Console.Write(dato);
            Usuario usuario =await this.repo.GetUsuarioLibre(dato);
            ViewData["USUARIO"] = usuario;
            ViewData["NUMAMIGOS"]=await this.repo.GetNumberAmigosAsync(usuario.IdUsuario);
            ViewData["LISTAAMIGOS"]=await this.repo.GetListaAmigosAsync(usuario.IdUsuario);
            Console.WriteLine(usuario.UserName);
            List<Camiseta> camisetas = await this.repo.GetCamisetasUsuario(usuario.IdUsuario);
            ViewData["CAMISETAS"] = camisetas;
            return View(usuario);
        }
        
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> PerfilUsuario(string alias, string equipo, IFormFile avatar, string contrasena)
        {
            Console.Write(alias+equipo+avatar.FileName+contrasena);
            int idUsuario = int.Parse(HttpContext.User.FindFirst("IDUSUARIO").Value);
            await this.repo.EditarPerfil(idUsuario, alias, avatar, equipo, contrasena);    
            ViewData["USUARIO"] = this.repo.GetUsuarioLibre(idUsuario);
            ViewData["NUMAMIGOS"]=await this.repo.GetNumberAmigosAsync(idUsuario);
            ViewData["LISTAAMIGOS"]=await this.repo.GetListaAmigosAsync(idUsuario);
            
            List<Camiseta> camisetas = await this.repo.GetCamisetasUsuario(idUsuario);
            ViewData["CAMISETAS"] = camisetas;
            return View();
        }
        //public async Task<IActionResult> DetalleCamiseta(int idCamiseta)
        //{
        //    Camiseta camiseta =await  this.repo.GetCamiseta(idCamiseta);
        //    List<Etiqueta> etiquetas=await this.repo.GetEtiquetas(camiseta.IdCamiseta);
        //    ViewData["ETIQUETAS"]=etiquetas;
        //    return View(camiseta);
        //}
        [AuthorizeUsers]
        public async Task<IActionResult> DetallesCamiseta(int idCamiseta)
        {
            Console.WriteLine(idCamiseta);
            List<Etiqueta> etiquetas = await this.repo.GetEtiquetas(idCamiseta);
            ViewData["ETIQUETAS"]=etiquetas;
            List<Comentario> comentarios = await this.repo.GetComentariosAsync(idCamiseta);
            Camiseta camiseta = await this.repo.GetCamiseta(idCamiseta);
            Console.WriteLine(camiseta.Equipo);
            ViewData["CAMISETA"] = camiseta;


            ViewData["COMENTARIOS"]=comentarios;

            return View();

        }
        [AuthorizeUsers]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult>DetallesCamiseta(int idCamiseta, string texto)
        {

            List<Comentario> comentarios = await this.repo.GetComentariosAsync(idCamiseta);
            Camiseta camiseta = await this.repo.GetCamiseta(idCamiseta);
            ViewData["CAMISETA"]=camiseta;
            ViewData["COMENTARIOS"]=comentarios;
            List<Etiqueta> etiquetas = await this.repo.GetEtiquetas(idCamiseta);
            ViewData["ETIQUETAS"]=etiquetas;
            Comentario comentario = new Comentario();
            comentario.IdComentario=await this.repo.GetMaxIdComment();
            comentario.ComentarioTxt=texto;
            comentario.FechaComentario=DateTime.Now;
            comentario.CamisetaId=idCamiseta;
            comentario.UsuarioId=int.Parse(HttpContext.User.FindFirst("IDUSUARIO").Value);

            await this.repo.Comentar(comentario);
            return RedirectToAction("DetallesCamiseta", new { idCamiseta });
        }
        public async Task<IActionResult>UpdateCamiseta(int idCamiseta)
        {
            Camiseta cam = await this.repo.GetCamiseta(idCamiseta);
            
            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewBag.Paises=paises;
            List<Etiqueta> etiquetas = await this.repo.GetEtiquetas(idCamiseta);
            string[] etiquetasArray = etiquetas.Select(e => e.TxtEtiqueta).ToArray();
            ViewData["ETIQUETAS"]=string.Join(",",etiquetasArray);
            return View(cam);
        }
        [AuthorizeUsers]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpdateCamiseta(int idCamiseta, string? equipo, string? pais, int? year, string? marca, string? equipacion, string? descripcion, string? condicion, int? dorsal, string?jugador, IFormFile? imagen)
        {
            int idUsuario = int.Parse(HttpContext.User.FindFirst("IDUSUARIO").Value);
            await this.repo.ModificarCamiseta( idCamiseta,year,marca,descripcion,condicion,dorsal,jugador,imagen,idUsuario);

            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewBag.Paises=paises;
            List<Etiqueta> etiquetas = await this.repo.GetEtiquetas(idCamiseta);
            string[] etiquetasArray = etiquetas.Select(e => e.TxtEtiqueta).ToArray();
            ViewData["ETIQUETAS"]=string.Join(",", etiquetasArray);
            return RedirectToAction("PerfilUsuario", new { idUsuario = idUsuario });
        }
        
        

        //[HttpPost]
        //public async Task<IActionResult> SubirImagenPerfil(IFormFile file)
        //{
        //    string fileName = file.FileName;
        //    string path = this.helper.MapPath(fileName, Folders.Avatar);
        //    using (Stream stream = new FileStream(path, FileMode.Create))
        //    {

        //        await file.CopyToAsync(stream);
        //    }
        //    ViewData["IMAGENPERFIL"]=this.helper.MapUrlPath(fileName, Folders.Avatar);
        //    return View();
        //}


        [AuthorizeUsers]
        public async  Task<IActionResult> CreateCamiseta()
        {
            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            return View();
        }
        [AuthorizeUsers]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateCamiseta(string equipo, string pais, int year, string marca, string equipacion, string condicion, int?dorsal, string?jugador,string? descripcion, IFormFile imagenCamiseta, string?etiquetas  )
        {
            //IDCAMISETA, ESACTIVA, FECHA SUBIDA, IDUSUARIO,
            Camiseta cam = new Camiseta();
            var idCamiseta=await this.repo.GetMaxIdCamiseta();
            cam.IdCamiseta=idCamiseta;
            cam.Equipo=equipo;
            cam.CodigoPais=pais;
            cam.Year=year;
            cam.Marca=marca;
            cam.Equipacion=equipacion;
            cam.Condicion=condicion;
            cam.Dorsal=dorsal;
            cam.Jugador=jugador;
            cam.EsActiva=1;
            int idUsuario = int.Parse(HttpContext.User.FindFirst("IDUSUARIO").Value);
            cam.IdUsuario=idUsuario;
            cam.Descripcion=descripcion;
            string filename = this.repo.GenerateUniqueFileName(idUsuario, imagenCamiseta);
            cam.Imagen=filename;
            cam.FechaSubida=DateTime.Now;
            await this.repo.SubirCamiseta(cam);
            await this.repo.SubirFichero(imagenCamiseta, Folders.Jerseys, filename);
            if (etiquetas!=null)
            {
                List<string> listaEtiquetas = etiquetas.ToUpper().Split(',').Select(e=>e.Trim()).ToList();
                Console.Write(listaEtiquetas);
                await this.repo.InsertEtiquetas(listaEtiquetas, idCamiseta);
            }
            
            

            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            return RedirectToAction("PerfilUsuario");
        }

        [AuthorizeUsers]
        public async Task<IActionResult>AgregarAmigo(int idAmigo)
        {
            //Comprobar que no son amigos
            //if
            //si esta bien agregar
            int idUsuario = int.Parse(HttpContext.User.FindFirst("IDUSUARIO").Value);
            if (await this.repo.AreAlreadyFriends(idUsuario, idAmigo))
            {
                TempData["ERROR"]="YA SOIS AMIGOS";
                return View();
            }
            else
            {
                await this.repo.SetAmistad(idUsuario, idAmigo);
                return RedirectToAction("PerfilUsuario");
            }
        }
        [AuthorizeUsers]
        public IActionResult BuscarAmigo()
        {
            return View();
        }
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult>BuscarAmigo(string codigoAmigo)
        {
            int idUsuario = int.Parse(HttpContext.User.FindFirst("IDUSUARIO").Value);
            var amigo = await this.repo.FindUsuarioAmistadCode(codigoAmigo);

            if (amigo.IdUsuario==idUsuario)
            {
                TempData["ERROR"]="No puedes agregarte a ti mismo.";
                return View();
            }
            else if(amigo==null)
            {
                TempData["ERROR"]="No se han encontrado resultados";
                return View();
            }
            else
            {
                return View(amigo);
            }

        }
        [AuthorizeUsers]
        public async Task<IActionResult> UsuarioPerfil(int amigoId)
        {
            
            ViewData["USUARIO"]=await this.repo.GetUsuarioLibre(amigoId);
            ViewData["NUMAMIGOS"]=await this.repo.GetNumberAmigosAsync(amigoId);
            ViewData["LISTAAMIGOS"]=await this.repo.GetListaAmigosAsync(amigoId);
            List<Camiseta> camisetas = await this.repo.GetCamisetasUsuario(amigoId);
            ViewData["CAMISETAS"] = camisetas;
            return View();
        }

        [AuthorizeUsers]
        public async Task<IActionResult>DeleteCamiseta(int idCamiseta)
        {
            await this.repo.DeleteCamiseta(idCamiseta);
            return RedirectToAction("PerfilUsuario");
        }

    }
}
