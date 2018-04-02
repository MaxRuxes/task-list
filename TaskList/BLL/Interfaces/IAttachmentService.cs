using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface IAttachmentService
    {
        void AddAttachment(TodoDTO todo, AttachmentsDTO attachments);
        void UpdateAttachment(AttachmentsDTO attachments);
        void DeleteAttachment(TodoDTO todo, AttachmentsDTO attachments);
        AttachmentsDTO GetAttachment(int? todoId, int? id);
        IEnumerable<AttachmentsDTO> GetAllAttachments(int? todoId);

        void Dispose();
    }
}
