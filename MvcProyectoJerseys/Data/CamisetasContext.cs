﻿using Microsoft.EntityFrameworkCore;
using MvcProyectoJerseys.Models;

namespace MvcProyectoJerseys.Data
{
    public class CamisetasContext : DbContext
    {
        public CamisetasContext(DbContextOptions<CamisetasContext> options) : base(options)
        {

        }
        //public DbSet<Camiseta> Camisetas { get; set; }
        //public DbSet<Usuario> Usuarios { get; set; }
        //public DbSet<Comentario> Comentarios { get; set; }
        //public DbSet<UsuarioPuro> UsuariosPuros { get; set;}
        //public DbSet<Pais> Paises { get; set; }

        //public DbSet<Like> Likes { get; set; }

        //public DbSet<Amistad> Amistades { get; set; }

        //public DbSet<Etiqueta> Etiquetas { get; set; }
        ////public DbSet<EtiquetaCamiseta> EtiquetaCamisetas { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<Comentario>().HasOne(c => c.Camiseta)
        //    //    .WithMany(c => c.Comentarios)
        //    //    .HasForeignKey(c => c.CamisetaId);
            
        //}


    }
}
