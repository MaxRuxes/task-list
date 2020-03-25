using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Entities
{
    [Table("todoandusers")]
    public class TodoAndUsers
    {
        [Column("idTodoAndUsers")]
        public int TodoAndUsersId { get; set; }

        [ForeignKey("User")]
        [Column("idUser")]
        public int Iduser { get; set; }

        [ForeignKey("Todo")]
        [Column("idTodo")]
        public int IdTodo { get; set; }

        public virtual User User { get; set; }

        public virtual Todo Todo { get; set; }
    }
}
