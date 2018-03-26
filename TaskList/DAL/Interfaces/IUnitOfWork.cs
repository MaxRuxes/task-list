using System;
using TaskList.DAL.Models;

namespace TaskList.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Attachments> Attachments { get; }
        IRepository<AttachmentType> AttachmentTypes { get; }
        IRepository<PriorityType> PriorityTypes { get; }
        IRepository<RolesType> RolesTypes { get; }
        IRepository<TeamInfo> TeamInfos { get; }
        IRepository<Teams> Teams { get; }
        IRepository<Todo> Todos { get; }
        IRepository<TodoAndAttaches> TodoAndAttaches { get; }
        IRepository<TodoAndUsers> TodoAndUsers { get; }
        IRepository<User> Users { get; }

        void Save();
    }
}
