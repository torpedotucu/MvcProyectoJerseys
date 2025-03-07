using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcProyectoJerseys.Models
{
    [Table("USUARIOS")]
    public class UsuarioPuro
    {
        [Key]
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }

        [Column("USERNAME")]
        public string UserName { get; set; }

        [Column("ALIAS")]
        public string AliasName { get; set; }

        [Column("EMAIL")]
        public string Correo { get; set; }

        [Column("SALT")]
        public string Salt { get; set; }

        [Column("PASS")]
        public byte[] Contrasena { get; set; }

        [Column("AVATAR")]
        public string Avatar { get; set; }

        [Column("EQUIPO")]
        public string Equipo { get; set; }

        [Column("PAIS")]
        public string Pais { get; set; }

        [Column("CODEAMISTAD")]
        public string CodeAmistad { get; set; }

        [Column("FECHA_UNION")]
        public DateTime FechaUnion { get; set; }
    }
}
