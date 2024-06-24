using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Certificado.Dominio.Entities
{
    [Table("Usuario")]
    public class Tb_Usuario
    {
        [Key]
        public int Fk_usuario { get; set; }

        [StringLength(30)]
        public string Nome_usuario { get; set; }

        [StringLength(30)]
        public string Senha_usuario { get; set; }
    }
}
