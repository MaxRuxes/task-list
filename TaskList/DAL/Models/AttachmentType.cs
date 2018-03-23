using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("attachmenttype")]
    public class AttachmentType
    {
        [Key]
        [Column("idAttachmentType")]
        public int IdAttachType { get; set; }

        [Column("typeContent")]
        public string NameType { get; set; }
    }
}
