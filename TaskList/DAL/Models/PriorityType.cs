using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("priority")]
    public class PriorityType
    {
        [Key]
        [Column("idPriority")]
        public int IdPriority { get; set; }

        [Column("content")]
        public string NamePriority { get; set; }
    }
}
