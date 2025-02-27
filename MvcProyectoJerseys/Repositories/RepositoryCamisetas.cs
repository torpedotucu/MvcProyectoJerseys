using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MvcProyectoJerseys.Data;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using System.Runtime.CompilerServices;

namespace MvcProyectoJerseys.Repositories
{

    public class RepositoryCamisetas
    {
        private CamisetasContext context;
        private IWebHostEnvironment hostEnvironment;
        private HelperPathProvider helperPathProvider;

        public RepositoryCamisetas(CamisetasContext context, IWebHostEnvironment hostEnvironment,HelperPathProvider helper) 
        {
            this.context=context;
            this.hostEnvironment=hostEnvironment;
            this.helperPathProvider=helper;
        }
        public Usuario? LoginUsuario(string username, string contrasena)
        {
            return context.Usuarios
                .FirstOrDefault(u => u.UserName == username && u.Contrasena == contrasena);
        }
        public Usuario GetUsuario(int idUsuario)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.IdUsuario == idUsuario
                           select datos;
            return consulta.FirstOrDefault();
        }

        public List<Camiseta> GetCamisetasUsuario(int idUsuario)
        {
            var consulta = from datos in this.context.Camisetas
                           where datos.IdUsuario== 2
                           select datos;

            List<Camiseta> camisetas = new List<Camiseta>();
            Console.WriteLine(consulta);
            foreach (var row in consulta)
            {
                Console.WriteLine(row.IdCamiseta);
                Camiseta cam = new Camiseta();
                cam.IdCamiseta = row.IdCamiseta;
                cam.IdUsuario = row.IdUsuario;
                cam.Equipo = row.Equipo;
                cam.Pais = row.Pais;
                cam.Year = row.Year;
                cam.Marca = row.Marca;
                cam.Equipacion = row.Equipacion;
                cam.Descripcion= row.Descripcion;
                cam.Posicion = row.Posicion;
                cam.Condicion = row.Condicion;
                cam.Dorsal = row.Dorsal;
                cam.Jugador = row.Jugador;
                cam.EsActiva = row.EsActiva;
                cam.FechaSubida = row.FechaSubida;
                cam.Imagen = row.Imagen;
                camisetas.Add(cam);
            }
            return camisetas;
        }
        public async Task<Camiseta> GetCamiseta(int idCamiseta)
        {
            var consulta = from datos in this.context.Camisetas
                           where datos.IdCamiseta == idCamiseta
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public void SubirCamiseta(Camiseta camiseta)
        {
            this.context.Camisetas.Add(camiseta);
            this.context.SaveChanges();
        }

        public void ModificarCamiseta(Camiseta camiseta)
        {
            this.context.Camisetas.Update(camiseta);
            this.context.SaveChanges();
        }

        public async Task EditarPerfil(UsuarioPuro u)
        {
            UsuarioPuro user = await this.context.UsuariosPuros.FirstOrDefaultAsync(c => c.IdUsuario == u.IdUsuario);
            if (user!=null)
            {
                user.IdUsuario = u.IdUsuario;
                user.UserName = u.UserName;
                user.Pais = u.Pais;
                user.AliasName = u.AliasName;
                user.Avatar = u.Avatar;
                user.Correo=u.Correo;
                user.Contrasena = u.Contrasena;
                user.Equipo = u.Equipo;
                await this.context.SaveChangesAsync();
            }
           

        }
        public async Task<List<Comentario>> GetComentariosAsync(int idCamiseta)
        {
            var consulta = from datos in this.context.Comentarios
                           where datos.CamisetaId==idCamiseta
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<CamisetaComentarios> DetalleCamiseta(int idCamiseta)
        {

            Camiseta camiseta = await this.GetCamiseta(idCamiseta);
            List<Comentario>comentarios=await this.GetComentariosAsync(idCamiseta);
            CamisetaComentarios camisetaComentarios = new CamisetaComentarios();
            camisetaComentarios.Camiseta=camiseta;
            camisetaComentarios.Comentarios=comentarios;
            return camisetaComentarios;
        }

        public async Task Comentar(Comentario comentario)
        {
            comentario.IdComentario=0;
            await this.context.Comentarios.AddAsync(comentario);
            await this.context.SaveChangesAsync();
        }


        public async Task<int> GetMaxIdCamiseta()
        {
            if (this.context.Camisetas.Count()==0)
            {
                return 1;
            }
            else
            {
                return await this.context.Camisetas.MaxAsync(x => x.IdCamiseta)+1;
            }
        }

        public async Task<int> GetMaxIdUsuario()
        {
            if (this.context.Usuarios.Count()==0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync(x => x.IdUsuario)+1;
            }
        }

        public async Task<int> GetMaxIdComment()
        {
            if (this.context.Comentarios.Count()==0)
            {
                return 1;
            }
            else
            {
                return await this.context.Comentarios.MaxAsync(x => x.IdComentario)+1;
            }
        }

        public async Task<List<Pais>> GetPaisesAsync()
        {
            var consulta = from datos in this.context.Paises
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task SubirFichero(IFormFile file,Folders folders)
        {
            string rootFolder = this.hostEnvironment.WebRootPath;
            string fileName = file.FileName;
            string path = this.helperPathProvider.MapPath(fileName, folders);

            using(Stream stream=new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public async Task CreateUsuario(UsuarioPuro usuario)
        {
            await this.context.UsuariosPuros.AddAsync(usuario);
            await this.context.SaveChangesAsync();
        }
    }
}
