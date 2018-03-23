using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("todoandfiles")]
    public class TodoAndAttaches
    {
        [Key]
        [Column("idtodoAndFiles")]
        public int IdTodoAndFiles { get; set; }

        [Key]
        [Column("idAttach")]
        public int IdAttach { get; set; }

        [Key]
        [Column("idTodo")]
        public int IdTodo { get; set; }
    }
}
