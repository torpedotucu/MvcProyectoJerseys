using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoJerseys.Models
{
    [Table("COMENTARIOS")]
    public class Comentario
    {
        [Key]
        [Column("IDCOMENTARIO")]
        public int IdComentario { get; set; }
        [Column("CAMISETAID")]
        public int CamisetaId { get; set; }
        [Column("USUARIOID")]
        public int UsuarioId { get; set; }
        [Column("TEXTOCOMENTARIO")]
        public string ComentarioTxt { get; set; }
        [Column("FECHACOMENTARIO")]
        public DateTime FechaComentario { get; set; }
        
        public Usuario Usuario { get; set; }
    }
}
