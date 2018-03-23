using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("attachmenttype")]
    public class AttachmentType
    {
        [Column("idAttachmentType")]
        public int AttachmentTypeId { get; set; }

        [Column("typeContent")]
        public string NameType { get; set; }
    }
}
