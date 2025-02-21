using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcProyectoJerseys.Models
{
    [Table("V_USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("UserName")]
        public string UserName { get; set; }

        [Column("AliasName")]
        public string AliasName { get; set; }

        [Column("correo")]
        public string Correo { get; set; }

        [Column("Contrasena")]
        public string Contrasena { get; set; }

        [Column("Avatar")]
        public string Avatar { get; set; }

        [Column("Equipo")]
        public string Equipo { get; set; }

        [Column("Pais")]
        public string Pais { get; set; }

        [Column("fechaUnion")]
        public DateTime FechaUnion { get; set; }
    }
}
