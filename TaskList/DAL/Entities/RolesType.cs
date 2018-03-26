using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("roles")]
    public class RolesType
    {
        [Column("idRoles")]
        public int RolesTypeId { get; set; }

        [Column("nameRole")]
        public string NameRole { get; set; }
    }
}
