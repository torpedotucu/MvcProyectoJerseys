using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcProyectoJerseys.Models
{
    
    public class CamisetaComentarios
    {

        public Camiseta Camiseta { get; set; }
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
