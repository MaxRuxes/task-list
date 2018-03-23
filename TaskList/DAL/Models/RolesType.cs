using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("roles")]
    public class RolesType
    {
        [Key]
        [Column("idRoles")]
        public int IdRole { get; set; }

        [Column("nameRole")]
        public string NameRole { get; set; }
    }
}
