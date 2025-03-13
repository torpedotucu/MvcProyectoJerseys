using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Extensions;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Repositories;

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
            UsuarioPuro user=HttpContext.Session.GetObject<UsuarioPuro>("USUARIO");
            
            UsuarioPuro usuario = await this.repo.GetUsuario(user.IdUsuario);
            ViewData["USUARIO"] = usuario;
            ViewData["NUMAMIGOS"]=await this.repo.GetNumberAmigosAsync(usuario.IdUsuario);
            ViewData["LISTAAMIGOS"]=await this.repo.GetListaAmigosAsync(usuario.IdUsuario);
            Console.WriteLine(usuario.UserName);
            List<Camiseta> camisetas = await this.repo.GetCamisetasUsuario(user.IdUsuario);
            ViewData["CAMISETAS"] = camisetas;
            return View(usuario);
        }
        public async Task<IActionResult> DetalleCamiseta(int idCamiseta)
        {
            Camiseta camiseta =await  this.repo.GetCamiseta(idCamiseta);
            return View(camiseta);
        }
        [HttpPost]
        public async Task<IActionResult>DetallesCamiseta(int idCamiseta, string texto)
        {

             
            List<Comentario> comentarios = await this.repo.GetComentariosAsync(idCamiseta);
            Camiseta camiseta = await this.repo.GetCamiseta(idCamiseta);
            ViewData["CAMISETA"]=camiseta;
            ViewData["COMENTARIOS"]=comentarios;
            Comentario comentario = new Comentario();
            comentario.IdComentario=await this.repo.GetMaxIdComment();
            comentario.ComentarioTxt=texto;
            comentario.FechaComentario=DateTime.Now;
            comentario.CamisetaId=idCamiseta;
            comentario.UsuarioId=this.HttpContext.Session.GetObject<UsuarioPuro>("USUARIO").IdUsuario;
            await this.repo.Comentar(comentario);
            return RedirectToAction("DetallesCamiseta", new { idCamiseta });
        }
        public async Task<IActionResult> UpdateCamiseta(int idCamiseta, string equipo, string pais, int year, string marca, string equipacion, string descripcion, int posicion, string condicion, int dorsal, string jugador, int esActiva, string imagen)
        {
            Camiseta camiseta = await this.repo.GetCamiseta(idCamiseta);
            camiseta.Equipo = equipo;
            camiseta.CodigoPais = pais;
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
        
        public IActionResult SubirImagenPerfil()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubirImagenPerfil(IFormFile file)
        {
            string fileName = file.FileName;
            string path = this.helper.MapPath(fileName, Folders.Avatar);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {

                await file.CopyToAsync(stream);
            }
            ViewData["IMAGENPERFIL"]=this.helper.MapUrlPath(fileName, Folders.Avatar);
            return View();
        }

        public async Task<IActionResult> DetallesCamiseta(int idCamiseta)
        {
            Console.WriteLine(idCamiseta);
            
            List<Comentario> comentarios = await this.repo.GetComentariosAsync(idCamiseta);
            Camiseta camiseta = await this.repo.GetCamiseta(idCamiseta);
            Console.WriteLine(camiseta.Equipo);
            ViewData["CAMISETA"] = camiseta;
            
            
                ViewData["COMENTARIOS"]=comentarios;
            
            return View();
            
        }

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
            //cam.Posicion=posicion;
            cam.Condicion=condicion;
            cam.Dorsal=dorsal;
            cam.Jugador=jugador;
            cam.EsActiva=1;
            cam.IdUsuario=HttpContext.Session.GetObject<UsuarioPuro>("USUARIO").IdUsuario;
            cam.Descripcion=descripcion;
            cam.Imagen=imagenCamiseta.FileName;
            cam.FechaSubida=DateTime.Now;
            if (etiquetas!=null)
            {
                List<string> listaEtiquetas = etiquetas.Split(',').Select(e=>e.Trim()).ToList();
                await this.repo.InsertEtiquetas(listaEtiquetas, idCamiseta);
            }
            
            await this.repo.SubirCamiseta(cam);
            await this.repo.SubirFichero(imagenCamiseta, Folders.Jerseys);

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
