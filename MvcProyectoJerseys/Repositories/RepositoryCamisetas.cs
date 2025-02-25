using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MvcProyectoJerseys.Data;
using MvcProyectoJerseys.Models;
using System.Runtime.CompilerServices;

namespace MvcProyectoJerseys.Repositories
{
    public class RepositoryCamisetas
    {
        private CamisetasContext context;
        private IWebHostEnvironment hostEnvironment;

        public RepositoryCamisetas(CamisetasContext context, IWebHostEnvironment hostEnvironment = null)
        {
            this.context=context;
            this.hostEnvironment=hostEnvironment;
        }
        public Usuario? LoginUsuario(string username, string contrasena)
        {
            return context.Usuarios
                .FirstOrDefault(u => u.UserName == username && u.Contrasena == contrasena);
        }
        public Usuario GetUsuario(int idUsuario)
        {
            Console.WriteLine("d");
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

        public void EditarPerfil(Usuario usuario)
        {
            this.context.Usuarios.Update(usuario);
            this.context.SaveChanges();

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
            this.context.Comentarios.Add(comentario);
        }

    }
}
