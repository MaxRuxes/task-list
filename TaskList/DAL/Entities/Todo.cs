using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("todo")]
    public class Todo
    {
        [Column("idTodo")]
        public int TodoId { get; set; }

        [ForeignKey("Priority")]
        [Column("idPriority")]
        public int IdPriority { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("startDate")]
        public DateTime StartDate { get; set; }

        [Column("endRealDate")]
        public DateTime EndRealDate { get; set; }

        [Column("estimatedHours")]
        public int EstimatedHours { get; set; }

        public virtual PriorityType Priority { get; set; }
    }
}
