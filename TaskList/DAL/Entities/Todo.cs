using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Entities
{
    [Table("todo")]
    public class Todo
    {
        [Column("idTodo")]
        public int TodoId { get; set; }

        [ForeignKey("Priority")]
        [Column("idPriority")]
        public int IdPriority { get; set; }

        [Column("caption")]
        public string Caption { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("estimatedTime")]
        public int EstimatedHours { get; set; }

        [Column("startDate")]
        public DateTime? StartDate { get; set; }

        [Column("endRealDate")]
        public DateTime? EndRealDate { get; set; }

        [Column("spentTime")]
        public int SpentTime { get; set; }

        [Column("state")]
        public int State { get; set; }

        public virtual PriorityType Priority { get; set; }
    }
}
