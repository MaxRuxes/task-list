using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.DAL.Models
{
    [Table("attachments")]
    public class Attachments
    {
        [Column("idAttachments")]
        public int AttachmentsId { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("idAttachType")]
        public int IdAttachType { get; set; }

        [Column("size")]
        public double? Size { get; set; }

        [Column("createDate")]
        public DateTime CreateDate { get; set; }
    }
}
