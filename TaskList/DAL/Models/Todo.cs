using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("todo")]
    public class Todo
    {
        [Key]
        [Column("idTodo")]
        public int IdTodo { get; set; }

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
    }
}
