using System;

namespace TaskList.BLL.DTO
{
    public class AttachmentsDTO
    {
        public int AttachmentsId { get; set; }
        public string Content { get; set; }
        public int AttachTypeId { get; set; }
        public double? Size { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
