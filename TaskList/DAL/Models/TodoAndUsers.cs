using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("todoandusers")]
    public class TodoAndUsers
    {
        [Key]
        [Column("idTodoAndUsers")]
        public int IdTodoAndUsers { get; set; }

        [Key]
        [Column("idUser")]
        public int Iduser { get; set; }

        [Key]
        [Column("idTodo")]
        public int IdTodo { get; set; }
    }
}
