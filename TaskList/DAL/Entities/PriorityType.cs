using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Entities
{
    [Table("priority")]
    public class PriorityType
    {
        [Column("idPriority")]
        public int PriorityTypeId { get; set; }

        [Column("content")]
        public string NamePriority { get; set; }
    }
}
