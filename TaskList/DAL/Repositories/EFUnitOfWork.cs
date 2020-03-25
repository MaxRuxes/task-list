using System;
using System.Data.Entity;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly TaskListContext _databaseContext;

        private PriorityRepository priorityRepository;
        private RolesRepository rolesRepository;
        private ProjectInfoRepository teamInfoRepository;
        private ProjectsRepository teamsRepository;
        private TodosRepository todosRepository;
        private TodoAndProjectsRepository todoAndProjectsRepository;
        private TodoAndUsersRepository todoAndUsersRepository;
        private UsersRepository usersRepository;

        private bool disposed = false;

        public EFUnitOfWork(string connectionString)
        {
            _databaseContext = new TaskListContext(connectionString);
        }

        public Database Database => _databaseContext.Database;

        public IRepository<PriorityType> PriorityTypes
        {
            get
            {
                if (priorityRepository == null)
                {
                    priorityRepository = new PriorityRepository(_databaseContext);
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
                    rolesRepository = new RolesRepository(_databaseContext);
                }

                return rolesRepository;
            }
        }

        public IRepository<ProjectInfo> TeamInfos
        {
            get
            {
                if (teamInfoRepository == null)
                {
                    teamInfoRepository = new ProjectInfoRepository(_databaseContext);
                }
                return teamInfoRepository;
            }
        }

        public IRepository<Projects> Teams
        {
            get
            {
                if (teamsRepository == null)
                {
                    teamsRepository = new ProjectsRepository(_databaseContext);
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
                    todosRepository = new TodosRepository(_databaseContext);
                }
                return todosRepository;
            }
        }

        public IRepository<TodoAndProjects> TodoAndProjects
        {
            get
            {
                if (todoAndProjectsRepository == null)
                {
                    todoAndProjectsRepository = new TodoAndProjectsRepository(_databaseContext);
                }
                return todoAndProjectsRepository;
            }
        }

        public IRepository<TodoAndUsers> TodoAndUsers
        {
            get
            {
                if (todoAndUsersRepository == null)
                {
                    todoAndUsersRepository = new TodoAndUsersRepository(_databaseContext);
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
                    usersRepository = new UsersRepository(_databaseContext);
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
                    _databaseContext.Dispose();
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
            _databaseContext.SaveChanges();
        }
    }
}
