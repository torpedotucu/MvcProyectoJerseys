using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcProyectoJerseys.Data;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
            //var consulta = from datos in this.context.UsuariosPuros
            //               where datos.Correo==email
            //               select datos;
            var consulta = this.context.UsuariosPuros.Where(x => x.Correo==email);
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
        public async Task<UsuarioPuro> GetUsuario(int idUsuario)
        {
            var consulta = from datos in this.context.UsuariosPuros
                           where datos.IdUsuario == idUsuario
                           select datos;
             return await consulta.FirstOrDefaultAsync();
        }
        public async Task<Usuario>GetUsuarioLibre(int idUsuario)
        {
            var consulta = await this.context.Usuarios
                .Where(x => x.IdUsuario==idUsuario).FirstOrDefaultAsync();
            return consulta;
        }

        public async Task<List<Camiseta>> GetCamisetasUsuario(int idUsuario)
        {
            
            var consulta = this.context.Camisetas.Where(c => c.IdUsuario==idUsuario);

            List<Camiseta> camisetas = await consulta.ToListAsync();
            //Console.WriteLine(consulta);
            //foreach (var row in consulta)
            //{
            //    Console.WriteLine(row.IdCamiseta);
            //    Camiseta cam = new Camiseta();
            //    cam.IdCamiseta = row.IdCamiseta;
            //    cam.IdUsuario = row.IdUsuario;
            //    cam.Equipo = row.Equipo;
            //    cam.CodigoPais = row.CodigoPais;
            //    cam.Year = row.Year;
            //    cam.Marca = row.Marca;
            //    cam.Equipacion = row.Equipacion;
            //    cam.Descripcion= row.Descripcion;
            //    cam.Posicion = row.Posicion;
            //    cam.Condicion = row.Condicion;
            //    cam.Dorsal = row.Dorsal;
            //    cam.Jugador = row.Jugador;
            //    cam.EsActiva = row.EsActiva;
            //    cam.FechaSubida = row.FechaSubida;
            //    cam.Imagen = row.Imagen;
            //    camisetas.Add(cam);
            //}
            return camisetas;
        }
        public async Task<Camiseta> GetCamiseta(int idCamiseta)
        {
            //var consulta = from datos in this.context.Camisetas
            //               where datos.IdCamiseta == idCamiseta
            //               select datos;
            var consulta = this.context.Camisetas.Include(p => p.Pais)
                .Where(c => c.IdCamiseta==idCamiseta);
            Camiseta camiseta = await consulta.FirstOrDefaultAsync();
            return camiseta;
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
            var comentarios = await this.context.Comentarios.Include(c => c.Usuario).Where(c => c.CamisetaId==idCamiseta).ToListAsync();
            return comentarios;
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
            if (this.context.UsuariosPuros.Count()==0)
            {
                return 1;
            }
            else
            {
                int id= await this.context.UsuariosPuros.MaxAsync(x => x.IdUsuario)+1;
                return id;
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

        public async Task<int> GetMaxIdLike()
        {
            if (this.context.Likes.Count()==0)
            {
                return 1;
            }
            else
            {
                return await this.context.Likes.MaxAsync(x => x.idLike)+1;
            }
        }

        public async Task<List<Pais>> GetPaisesAsync()
        {
            var consulta = from datos in this.context.Paises
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<int> GetMaxIdAmistad()
        {
            if (this.context.Amistades.Count()==0)
            {
                return 1;
            }
            else
            {
                return await this.context.Amistades.MaxAsync(x => x.IdAmistad)+1;
            }
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
            //usuario.CodeAmistad=GenerateCodeAmistadUsuario();
            await this.context.UsuariosPuros.AddAsync(usuario);
            await this.context.SaveChangesAsync();
        }

        public  string GenerateCodeAmistadUsuario()
        {
            return Guid.NewGuid().ToString().Substring(0, 9).ToUpper();

        }

        public async Task<UsuarioPuro>FindUsuarioAmistadCode(string friendCode)
        {
            var consulta = await this.context.UsuariosPuros.Where(x => x.CodeAmistad==friendCode).FirstOrDefaultAsync();
            return consulta;
        }
        
        public async Task<bool>AreAlreadyFriends(int usuarioA, int UsuarioB)
        {
            return await this.context.Amistades.AnyAsync
                (a => (a.UsuarioId==usuarioA && a.AmigoId==UsuarioB) ||
                (a.UsuarioId==UsuarioB && a.AmigoId==usuarioA)
            );
        }

        public async Task SetAmistad(int userAId, int userBId)
        {
            var newAmistad = new Amistad
            {
                IdAmistad=await GetMaxIdAmistad(),
                UsuarioId=userAId,
                AmigoId=userBId,
                FechaAmistad=DateTime.UtcNow.Date
            };
            await this.context.Amistades.AddAsync(newAmistad);
            await this.context.SaveChangesAsync();
        }

        public async Task<int>GetNumberAmigosAsync(int idUsuario)
        {
            var consulta = await this.context.Amistades
                .Where(a => a.UsuarioId==idUsuario || a.AmigoId==idUsuario)
                .CountAsync();
            return consulta;
        }
        
        public async Task<List<Usuario>>GetListaAmigosAsync(int idUsuario)
        {
            var sql = "SP_OBTENER_AMIGOS_USUARIO @idusuario";
            SqlParameter paramIdUsuario = new SqlParameter("@idusuario", idUsuario);
            var consulta = this.context.Usuarios.FromSqlRaw(sql, paramIdUsuario);
            List<Usuario> amigos = await consulta.ToListAsync();
            return amigos;
        }


        //PARA VERSION DOS
        //COMPROBAR QUE EL USUARIO TIENE LIKE
        public async Task AddLike(int idUser,int idCamiseta)
        {
            Like like = new Like
            {
                idLike=await this.GetMaxIdLike(),
                idUsuario=idUser,
                idCamiseta=idCamiseta,
                FechaLike=DateTime.Now
            };
            await this.context.Likes.AddAsync(like);
            await this.context.SaveChangesAsync();
        }
        public async Task HadLike(int camisetaId, int userId)
        {
            var like = await this.context.Likes.FirstOrDefaultAsync(l => l.idUsuario==userId && l.idCamiseta==camisetaId);
            if (like==null)
            {
                this.AddLike(userId, camisetaId);
            }
            else
            {
                this.context.Likes.Remove(like);
                await this.context.SaveChangesAsync();
            }
            
        }

        public async Task InsertEtiquetas(List<string> etiquetas, int idCamiseta)
        {
            foreach(var etiqueta in etiquetas)
            {
                var sql = "SP_INSERT_ETIQUETA_CAMISETA @nombreEtiqueta, @idCamiseta";
                SqlParameter paramNombre = new SqlParameter("@nombreEtiqueta", etiqueta);
                SqlParameter paramCamiseta = new SqlParameter("@idCamiseta", idCamiseta);
                this.context.Etiquetas.FromSqlRaw(sql, paramNombre, paramCamiseta);
            }
        }

        

        
    }
}
