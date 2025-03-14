using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProyectoJerseys.Models
{
    [Table("CAMISETA_ETIQUETA")]
    public class EtiquetaCamiseta
    {
        [Column("idEtiqueta")]
        public string IdEtiqueta { get; set; }
        [Column("idCamiseta")]
        public string IdCamiseta { get; set; }
    }
}
