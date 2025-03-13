using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcProyectoJerseys.Models
{
    [Table("CAMISETA")]
    public class Camiseta
    {
        [Key]
        [Column("IdCamiseta")]
        public int IdCamiseta { get; set; }

        [Column("Equipo")]
        public string Equipo { get; set; }

        [Column("CodigoPais")]
        public string CodigoPais { get; set; }

        [Column("Year")]
        public int Year { get; set; }

        [Column("Marca")]
        public string Marca { get; set; }

        [Column("Equipacion")]
        public string Equipacion { get; set; }

        [Column("Posicion")]
        public int? Posicion { get; set; }

        [Column("Condicion")]
        public string Condicion { get; set; }

        [Column("Dorsal")]
        public int? Dorsal { get; set; } // Puede ser nulo si no todas las camisetas tienen número

        [Column("Jugador")]
        public string? Jugador { get; set; }

        [Column("Es_Activa")]
        public int EsActiva { get; set; }

        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("FechaSubida")]
        public DateTime FechaSubida { get; set; }

        [Column("Descripcion")]
        public string? Descripcion { get; set; }
        [Column("ImagenCamiseta")]
        public string? Imagen { get; set; }

        public List<Comentario> Comentarios { get; set; }

        [ForeignKey("CodigoPais")]
        public Pais Pais { get; set; }
    }
}
