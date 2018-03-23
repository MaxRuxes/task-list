using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("todoandusers")]
    public class TodoAndUsers
    {
        [Column("idTodoAndUsers")]
        public int TodoAndUsersId { get; set; }

        [Column("idUser")]
        public int Iduser { get; set; }

        [Column("idTodo")]
        public int IdTodo { get; set; }
    }
}
