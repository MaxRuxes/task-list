using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("todoandfiles")]
    public class TodoAndAttaches
    {
        [Column("idtodoAndFiles")]
        public int TodoAndAttachesId { get; set; }

        [Column("idAttach")]
        public int IdAttach { get; set; }

        [Column("idTodo")]
        public int IdTodo { get; set; }

        public virtual Attachments Attachments { get; set; }

        public virtual Todo Todo { get; set; }
    }
}
