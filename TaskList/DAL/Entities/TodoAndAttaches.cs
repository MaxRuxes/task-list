using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("todoandfiles")]
    public class TodoAndAttaches
    {
        [Column("idtodoAndFiles")]
        public int TodoAndAttachesId { get; set; }

        [ForeignKey("Attachment")]
        [Column("idAttach")]
        public int IdAttach { get; set; }

        [ForeignKey("Todo")]
        [Column("idTodo")]
        public int IdTodo { get; set; }

        public virtual Attachments Attachment { get; set; }

        public virtual Todo Todo { get; set; }
    }
}
