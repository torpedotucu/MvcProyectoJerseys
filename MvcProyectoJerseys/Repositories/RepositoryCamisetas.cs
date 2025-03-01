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
        public async Task<UsuarioPuro?> LoginUsuario(string email, string contrasena)
        {
            var consulta = from datos in this.context.UsuariosPuros
                           where datos.Correo==email
                           select datos;
            UsuarioPuro user = await consulta.FirstOrDefaultAsync();
            if (user==null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp = HelperCryptography.EncryptPassword(contrasena, salt);
                byte[] passBytes = user.Contrasena;
                bool response = HelperCryptography.CompararArrays(temp, passBytes);
                if (response==true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
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

        public async Task<int> SubirCamiseta(Camiseta camiseta)
        {
            await this.context.Camisetas.AddAsync(camiseta);
            await this.context.SaveChangesAsync();
            return camiseta.IdCamiseta;
        }

        public void ModificarCamiseta(Camiseta camiseta)
        {
            this.context.Camisetas.Update(camiseta);
            this.context.SaveChanges();
        }


        //REVISAR
        public async Task EditarPerfil(UsuarioPuro u)
        {
            //UsuarioPuro user = await this.context.UsuariosPuros.FirstOrDefaultAsync(c => c.IdUsuario == u.IdUsuario);
            //if (user!=null)
            //{
            //    user.IdUsuario = u.IdUsuario;
            //    user.UserName = u.UserName;
            //    user.Pais = u.Pais;
            //    user.AliasName = u.AliasName;
            //    user.Avatar = u.Avatar;
            //    user.Correo=u.Correo;
            //    user.Salt=u.Salt;
            //    user.Contrasena = HelperCryptography.EncryptPassword(u.Contrasena,u.Salt);//REVISAR
            //    user.Equipo = u.Equipo;
            //    user.FechaUnion=u.FechaUnion;
            //    await this.context.SaveChangesAsync();
            //}
           
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
