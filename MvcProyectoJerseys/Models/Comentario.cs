using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoJerseys.Models
{
    [Table("comentario")]
    public class Comentario
    {
        [Key]
        [Column("idComentario")]
        public int IdComentario { get; set; }
        [Column("camisetaId")]
        public int CamisetaId { get; set; }
        [Column("usuarioId")]
        public int UsuarioId { get; set; }
        [Column("textoComentario")]
        public string ComentarioTxt { get; set; }
        [Column("fechaComentario")]
        public DateTime FechaComentario { get; set; }
        public Camiseta Camiseta { get; set; }
    }
}
