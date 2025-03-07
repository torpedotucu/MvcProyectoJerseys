using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoJerseys.Models
{
    [Table("AMISTAD")]
    public class Amistad
    {
        [Key]
        [Column("IDAMISTAD")]
        public int IdAmistad { get; set; }
        [Column("USUARIOID")]
        public int UsuarioId { get; set; }
        [Column("AMIGOID")]
        public int AmigoId { get; set; }
        [Column("FECHAAMISTAD")]
        public DateTime FechaAmistad{get;set;}
    }
}
