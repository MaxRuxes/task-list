using System;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<PriorityType> PriorityTypes { get; }
        IRepository<RolesType> RolesTypes { get; }
        IRepository<ProjectInfo> TeamInfos { get; }
        IRepository<Projects> Teams { get; }
        IRepository<Todo> Todos { get; }
        IRepository<TodoAndProjects> TodoAndProjects { get; }
        IRepository<TodoAndUsers> TodoAndUsers { get; }
        IRepository<User> Users { get; }

        void Save();
    }
}
