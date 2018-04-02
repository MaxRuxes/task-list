using System;
using System.Data.Entity;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private TaskListContext db;

        private AttachmentsRepository attachmentsRepository;
        private AttachmentTypeRepository attachmentTypeRepository;
        private PriorityRepository priorityRepository;
        private RolesRepository rolesRepository;
        private TeamInfoRepository teamInfoRepository;
        private TeamsRepository teamsRepository;
        private TodosRepository todosRepository;
        private TodoAndAttachesRepository todoAndAttachesRepository;
        private TodoAndUsersRepository todoAndUsersRepository;
        private UsersRepository usersRepository;

        private bool disposed = false;

        public EFUnitOfWork(string connectionString)
        {
            db = new TaskListContext(connectionString);
        }

        public Database Database => db.Database;

        public IRepository<Attachments> Attachments
        {
            get
            {
                if (attachmentsRepository == null)
                {
                    attachmentsRepository = new AttachmentsRepository(db);
                }
                return attachmentsRepository;
            }
        }

        public IRepository<AttachmentType> AttachmentTypes
        {
            get
            {
                if (attachmentTypeRepository == null)
                {
                    attachmentTypeRepository = new AttachmentTypeRepository(db);
                }
                return attachmentTypeRepository;
            }
        }

        public IRepository<PriorityType> PriorityTypes
        {
            get
            {
                if (priorityRepository == null)
                {
                    priorityRepository = new PriorityRepository(db);
                }
                return priorityRepository;
            }
        }

        public IRepository<RolesType> RolesTypes
        {
            get
            {
                if (rolesRepository == null)
                {
                    rolesRepository = new RolesRepository(db);
                }
                return rolesRepository;
            }
        }

        public IRepository<TeamInfo> TeamInfos
        {
            get
            {
                if (teamInfoRepository == null)
                {
                    teamInfoRepository = new TeamInfoRepository(db);
                }
                return teamInfoRepository;
            }
        }

        public IRepository<Teams> Teams
        {
            get
            {
                if (teamsRepository == null)
                {
                    teamsRepository = new TeamsRepository(db);
                }
                return teamsRepository;
            }
        }

        public IRepository<Todo> Todos
        {
            get
            {
                if (todosRepository == null)
                {
                    todosRepository = new TodosRepository(db);
                }
                return todosRepository;
            }
        }

        public IRepository<TodoAndAttaches> TodoAndAttaches
        {
            get
            {
                if (todoAndAttachesRepository == null)
                {
                    todoAndAttachesRepository = new TodoAndAttachesRepository(db);
                }
                return todoAndAttachesRepository;
            }
        }

        public IRepository<TodoAndUsers> TodoAndUsers
        {
            get
            {
                if (todoAndUsersRepository == null)
                {
                    todoAndUsersRepository = new TodoAndUsersRepository(db);
                }
                return todoAndUsersRepository;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (usersRepository == null)
                {
                    usersRepository = new UsersRepository(db);
                }
                return usersRepository;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
