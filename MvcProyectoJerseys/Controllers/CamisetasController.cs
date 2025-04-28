using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Extensions;
using MvcProyectoJerseys.Filters;
using MvcProyectoJerseys.Helpers;

using MvcProyectoJerseys.Repositories;
using MvcProyectoJerseys.Services;
using NugetJerseyHubRGO.Models;

namespace MvcProyectoJerseys.Controllers
{
    public class CamisetasController : Controller
    {
        RepositoryCamisetas repo;
        HelperPathProvider helper;
        private ServiceStorageBlobs serviceBlobs;
        private ServiceCamisetas service;
        public CamisetasController(RepositoryCamisetas repo, HelperPathProvider helper, ServiceStorageBlobs serviceBlobs, ServiceCamisetas service)
        {
            this.repo = repo;
            this.serviceBlobs=serviceBlobs;
            this.service=service;
            this.helper=helper;
        }
        [AuthorizeUsers]
        public async Task<IActionResult> Index()
        {
            //Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            //int idUsuario = int.Parse(HttpContext.User.FindFirst("IDUSUARIO").Value);
            List<Camiseta> camisetas = await this.service.GetPublicacionesInicio();
            return View(camisetas);
        }
        [AuthorizeUsers]
        public async Task<IActionResult> PerfilUsuario()
        {
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            //int dato =int.Parse( HttpContext.User.FindFirst("IDUSUARIO").Value);

            //Usuario usuario = await this.service.GetUsuario();
            ViewData["USUARIO"] = usuario;

            List<Usuario> amigos = await this.service.GetListaAmigosAsync(usuario.IdUsuario);

            ViewData["NUMAMIGOS"]=amigos.Count;
            ViewData["LISTAAMIGOS"]=amigos;
            Console.WriteLine(usuario.UserName);
            List<Camiseta> camisetas = await this.service.GetCamisetasUsuario();
            ViewData["CAMISETAS"] = camisetas;
            return View(usuario);
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> PerfilUsuario(string alias, string equipo, IFormFile avatar, string? contrasena)
        {
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            //int dato =int.Parse( HttpContext.User.FindFirst("IDUSUARIO").Value);
            if (avatar!=null)
            {
                string nombreArchivo = this.repo.GenerateUniqueFileName(usuario.IdUsuario, avatar);
                await this.repo.SubirFichero(avatar, Folders.Avatar, nombreArchivo);
                await this.serviceBlobs.DeleteBlobAsync("camisetas", usuario.Avatar);
            }
            if (avatar != null)
            {
                string nombreArchivo = this.repo.GenerateUniqueFileName(usuario.IdUsuario, avatar);
                await this.repo.SubirFichero(avatar, Folders.Avatar, nombreArchivo);
                await this.serviceBlobs.DeleteBlobAsync("camisetas", usuario.Avatar); // Borra el antiguo
            }
            UsuarioUpdateDTO usuarioUpdate = new UsuarioUpdateDTO
            {
                Alias=alias,
                Equipo=equipo,
                Contrasena=contrasena
            };
            await this.service.EditarPerfil(usuarioUpdate);
            //Usuario usuario = await this.service.GetUsuario();
            ViewData["USUARIO"] = usuario;

            List<Usuario> amigos = await this.service.GetListaAmigosAsync(usuario.IdUsuario);
            ViewData["NUMAMIGOS"]=amigos.Count;
            ViewData["LISTAAMIGOS"]=amigos;
            Console.WriteLine(usuario.UserName);
            List<Camiseta> camisetas = await this.service.GetCamisetasUsuario();
            ViewData["CAMISETAS"] = camisetas;
            return View(usuario);

        }

        [AuthorizeUsers]
        public async Task<IActionResult> DetallesCamiseta(int idCamiseta)
        {
            Console.WriteLine(idCamiseta);
            List<Etiqueta> etiquetas = await this.service.GetEtiquetas(idCamiseta);
            ViewData["ETIQUETAS"]=etiquetas;
            List<Comentario> comentarios = await this.service.GetComentariosAsync(idCamiseta);
            Camiseta camiseta = await this.service.GetCamiseta(idCamiseta);
            Console.WriteLine(camiseta.Equipo);
            ViewData["CAMISETA"] = camiseta;


            ViewData["COMENTARIOS"]=comentarios;

            return View();

        }
        [AuthorizeUsers]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DetallesCamiseta(int idCamiseta, string texto)
        {
            List<Comentario> comentarios = await this.service.GetComentariosAsync(idCamiseta);
            Camiseta camiseta = await this.service.GetCamiseta(idCamiseta);
            ViewData["CAMISETA"]=camiseta;
            ViewData["COMENTARIOS"]=comentarios;
            List<Etiqueta> etiquetas = await this.service.GetEtiquetas(idCamiseta);
            ViewData["ETIQUETAS"]=etiquetas;

            ComentarioDTO comentarioDTO = new ComentarioDTO
            {
                CamisetaId=idCamiseta,
                ComentarioTxt=texto,
            };
            await this.service.Comentar(comentarioDTO);
            return RedirectToAction("DetallesCamiseta", new { idCamiseta });
        }
        public async Task<IActionResult> UpdateCamiseta(int idCamiseta)
        {
            Camiseta cam = await this.service.GetCamiseta(idCamiseta);

            List<Pais> paises = await this.service.GetPaisesAsync();
            ViewBag.Paises=paises;
            List<Etiqueta> etiquetas = await this.service.GetEtiquetas(idCamiseta);
            string[] etiquetasArray = etiquetas.Select(e => e.TxtEtiqueta).ToArray();
            ViewData["ETIQUETAS"]=string.Join(",", etiquetasArray);
            return View(cam);
        }
        [AuthorizeUsers]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpdateCamiseta(int idCamiseta, string? equipo, string? pais, int? year, string? marca, string? equipacion, string? descripcion, string? condicion, int? dorsal, string? jugador, IFormFile? imagen)
        {
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");

            Camiseta cam = await this.service.GetCamiseta(idCamiseta);
            if (imagen!=null)
            {
                string nombreArchivo = this.repo.GenerateUniqueFileName(usuario.IdUsuario, imagen);
                await this.repo.SubirFichero(imagen, Folders.Jerseys, nombreArchivo);
                await this.serviceBlobs.DeleteBlobAsync("camisetas", cam.Imagen);
            }

            CamisetaUpdateDTO camisetaUpdateDTO = new CamisetaUpdateDTO
            {
                IdCamiseta=idCamiseta,
                Year=year,
                Marca=marca,
                Condicion=condicion,

                Dorsal=dorsal,
                Jugador=jugador,
                Descripcion=descripcion,

            };
            await this.service.ModificarCamiseta(camisetaUpdateDTO);
            //await this.repo.ModificarCamiseta( idCamiseta,year,marca,descripcion,condicion,dorsal,jugador,imagen,idUsuario);

            List<Pais> paises = await this.service.GetPaisesAsync();
            ViewBag.Paises=paises;
            List<Etiqueta> etiquetas = await this.service.GetEtiquetas(idCamiseta);
            string[] etiquetasArray = etiquetas.Select(e => e.TxtEtiqueta).ToArray();
            ViewData["ETIQUETAS"]=string.Join(",", etiquetasArray);
            return RedirectToAction("PerfilUsuario", new { idUsuario = usuario.IdUsuario });
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
        public async Task<IActionResult> CreateCamiseta()
        {
            List<Pais> paises = await this.service.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            return View();
        }
        [AuthorizeUsers]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateCamiseta(string equipo, string pais, int year, string marca, string equipacion, string condicion, int? dorsal, string? jugador, string? descripcion, IFormFile imagenCamiseta, string? etiquetas)
        {
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            //IDCAMISETA, ESACTIVA, FECHA SUBIDA, IDUSUARIO,

            string filename = this.repo.GenerateUniqueFileName(usuario.IdUsuario, imagenCamiseta);
            await this.repo.SubirFichero(imagenCamiseta, Folders.Jerseys, filename);

            CamisetaCreateDTO camisetaCreateDTO = new CamisetaCreateDTO
            {
                Equipo=equipo,
                CodigoPais=pais,
                Year=year,
                Marca=marca,
                Equipacion=equipacion,
                Condicion=condicion,
                Dorsal=dorsal,
                Jugador=jugador,
                Descripcion=descripcion,
                Imagen=filename
            };
            int idCamiseta = await this.service.SubirCamiseta(camisetaCreateDTO);
            //await this.service.UploadBlobAsync("martes",imagenCamiseta.FileName,imagenCamiseta.OpenReadStream());
            if (etiquetas!=null)
            {
                List<string> listaEtiquetas = etiquetas.ToUpper().Split(',').Select(e => e.Trim()).ToList();
                Console.Write(listaEtiquetas);
                await this.service.InsertEtiquetas(listaEtiquetas, idCamiseta);
            }
            List<Pais> paises = await this.service.GetPaisesAsync();
            ViewData["PAISES"]=paises;
            return RedirectToAction("PerfilUsuario");
        }

        [AuthorizeUsers]
        public async Task<IActionResult> AgregarAmigo(int idAmigo)
        {

            //Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            if (await this.service.AreAlreadyFriends(idAmigo))
            {
                TempData["ERROR"]="YA SOIS AMIGOS";
                return View();
            }
            else
            {
                await this.service.SetAmistad(idAmigo);
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
        public async Task<IActionResult> BuscarAmigo(string codigoAmigo)
        {
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            var amigo = await this.service.FindUsuarioAmistadCode(codigoAmigo);

            if (amigo.IdUsuario==usuario.IdUsuario)
            {
                TempData["ERROR"]="No puedes agregarte a ti mismo.";
                return View();
            }
            else if (amigo==null)
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
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            List<Usuario> amigos = await this.service.GetListaAmigosAsync(usuario.IdUsuario);
            ViewData["USUARIO"]=usuario;
            ViewData["NUMAMIGOS"]=amigos.Count;
            ViewData["LISTAAMIGOS"]=amigos;
            List<Camiseta> camisetas = await this.service.GetCamisetasUsuario(amigoId);
            ViewData["CAMISETAS"] = camisetas;
            return View();
        }

        [AuthorizeUsers]
        public async Task<IActionResult> DeleteCamiseta(int idCamiseta)
        {
            await this.service.DeleteCamiseta(idCamiseta);
            return RedirectToAction("PerfilUsuario");
        }

    }
}
