using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MvcProyectoJerseys.Extensions;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;
using System.ComponentModel;

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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PerfilUsuario()
        {   
            Usuario usuario=HttpContext.Session.GetObject<Usuario>("USUARIO");
            
            ViewData["USUARIO"] = usuario;
            ViewData["NUMAMIGOS"]=await this.repo.GetNumberAmigosAsync(usuario.IdUsuario);
            ViewData["LISTAAMIGOS"]=await this.repo.GetListaAmigosAsync(usuario.IdUsuario);
            Console.WriteLine(usuario.UserName);
            List<Camiseta> camisetas = await this.repo.GetCamisetasUsuario(usuario.IdUsuario);
            ViewData["CAMISETAS"] = camisetas;
            return View(usuario);
        }
        [HttpPost]
        public async Task<IActionResult> PerfilUsuario(string alias, string equipo, IFormFile avatar, string contrasena)
        {
            Console.Write(alias+equipo+avatar.FileName+contrasena);
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            await this.repo.EditarPerfil(usuario.IdUsuario, alias, avatar, equipo, contrasena);    
            ViewData["USUARIO"] = usuario;
            ViewData["NUMAMIGOS"]=await this.repo.GetNumberAmigosAsync(usuario.IdUsuario);
            ViewData["LISTAAMIGOS"]=await this.repo.GetListaAmigosAsync(usuario.IdUsuario);
            Console.WriteLine(usuario.UserName);
            List<Camiseta> camisetas = await this.repo.GetCamisetasUsuario(usuario.IdUsuario);
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
            comentario.UsuarioId=this.HttpContext.Session.GetObject<Usuario>("USUARIO").IdUsuario;

            await this.repo.Comentar(comentario);
            return RedirectToAction("DetallesCamiseta", new { idCamiseta });
        }
        public async Task<IActionResult>UpdateCamiseta(int idCamiseta)
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCamiseta(int idCamiseta, string equipo, string pais, int year, string marca, string equipacion, string descripcion, string condicion, int dorsal, string jugador, string imagen)
        {
            Camiseta camiseta = await this.repo.GetCamiseta(idCamiseta);
            camiseta.Equipo = equipo;
            camiseta.CodigoPais = pais;
            camiseta.Year = year;
            camiseta.Marca = marca;
            camiseta.Equipacion = equipacion;
            camiseta.Descripcion = descripcion;
            
            camiseta.Condicion = condicion;
            camiseta.Dorsal = dorsal;
            camiseta.Jugador = jugador;
            
            camiseta.FechaSubida = DateTime.Now;
            camiseta.Imagen = imagen;
            Console.Write(camiseta);
            //this.repo.ModificarCamiseta(camiseta);
            return RedirectToAction("PerfilUsuario", new { idUsuario = camiseta.IdUsuario });
        }
        
        public IActionResult SubirImagenPerfil()
        {
            return View();
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

        

        public async  Task<IActionResult> CreateCamiseta()
        {
            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            return View();
        }

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
            int idUser= HttpContext.Session.GetObject<Usuario>("USUARIO").IdUsuario;
            cam.IdUsuario=idUser;
            cam.Descripcion=descripcion;
            string filename = this.repo.GenerateUniqueFileName(idUser, imagenCamiseta);
            cam.Imagen=filename;
            cam.FechaSubida=DateTime.Now;
            await this.repo.SubirCamiseta(cam);
            await this.repo.SubirFichero(imagenCamiseta, Folders.Jerseys, filename);
            if (etiquetas!=null)
            {
                List<string> listaEtiquetas = etiquetas.Split(',').Select(e=>e.Trim()).ToList();
                Console.Write(listaEtiquetas);
                await this.repo.InsertEtiquetas(listaEtiquetas, idCamiseta);
            }
            
            

            List<Pais> paises = await this.repo.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            return RedirectToAction("PerfilUsuario");
        }

        public async Task<IActionResult>AgregarAmigo(int idAmigo)
        {
            //Comprobar que no son amigos
            //if
            //si esta bien agregar
            int idUser= HttpContext.Session.GetObject<UsuarioPuro>("USUARIO").IdUsuario;
            if (await this.repo.AreAlreadyFriends(idUser, idAmigo))
            {
                TempData["ERROR"]="YA SOIS AMIGOS";
                return View();
            }
            else
            {
                await this.repo.SetAmistad(idUser, idAmigo);
                return RedirectToAction("PerfilUsuario");
            }
        }
        public IActionResult BuscarAmigo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>BuscarAmigo(string codigoAmigo)
        {
            var usuarioActualId = HttpContext.Session.GetObject<UsuarioPuro>("USUARIO").IdUsuario;
            var amigo = await this.repo.FindUsuarioAmistadCode(codigoAmigo);

            if (amigo.IdUsuario==usuarioActualId)
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

        public async Task<IActionResult> UsuarioPerfil(int amigoId)
        {
            Usuario user = await this.repo.GetUsuarioLibre(amigoId);
            ViewData["USUARIO"]=user;
            ViewData["NUMAMIGOS"]=await this.repo.GetNumberAmigosAsync(amigoId);
            ViewData["LISTAAMIGOS"]=await this.repo.GetListaAmigosAsync(amigoId);
            List<Camiseta> camisetas = await this.repo.GetCamisetasUsuario(amigoId);
            ViewData["CAMISETAS"] = camisetas;
            return View();
        }

    }
}
