using Microsoft.EntityFrameworkCore;
using MvcProyectoJerseys.Models;

namespace MvcProyectoJerseys.Data
{
    public class CamisetasContext:DbContext
    {
        public CamisetasContext(DbContextOptions<CamisetasContext> options) : base(options)
        {
        }
        public DbSet<Camiseta> Camisetas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
