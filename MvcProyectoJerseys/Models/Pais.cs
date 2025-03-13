using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoJerseys.Models
{
    [Table("paises")]
    public class Pais
    {
        [Key]
        [Column("codigo")]
        public string CodigoPais { get; set; }
        [Column("nombre")]
        public string NombrePais { get; set; }

        //public Camiseta Camiseta { get; set; }
    }
}
