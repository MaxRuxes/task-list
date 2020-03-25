using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Entities
{
    [Table("todoandprojects")]
    public class TodoAndProjects
    {
        [Column("idTasks")]
        public int TodoAndProjectsId { get; set; }

        [ForeignKey("Project")]
        [Column("idProject")]
        public int IdProject { get; set; }

        [ForeignKey("Todo")]
        [Column("idTodo")]
        public int IdTodo { get; set; }

        public virtual ProjectInfo Project { get; set; }

        public virtual Todo Todo { get; set; }
    }
}
