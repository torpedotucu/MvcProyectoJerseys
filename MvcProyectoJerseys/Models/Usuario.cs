using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcProyectoJerseys.Models
{
    [Table("V_USUARIOS_FREE")]
    public class Usuario
    {
        [Key]
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

        [Column("USERNAME")]
        public string UserName { get; set; }

        [Column("ALIAS")]
        public string Alias { get; set; }

        [Column("EMAIL")]
        public string Correo { get; set; }

        //[Column("Contrasena")]
        //public string Contrasena { get; set; }

        [Column("AVATAR")]
        public string Avatar { get; set; }

        [Column("EQUIPO")]
        public string Equipo { get; set; }

        [Column("CODEAMISTAD")]
        public string CodeAmistad { get; set; }

        [Column("Pais")]
        public string Pais { get; set; }

        [Column("FECHAUNION")]
        public DateTime FechaUnion { get; set; }
    }
}
