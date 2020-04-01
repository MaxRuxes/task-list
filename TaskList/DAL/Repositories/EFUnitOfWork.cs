using System;
using System.Data.Entity;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly TaskListContext _databaseContext;

        private PriorityRepository _priorityRepository;
        private RolesRepository _rolesRepository;
        private ProjectInfoRepository _teamInfoRepository;
        private ProjectsRepository _teamsRepository;
        private TodosRepository _todosRepository;
        private TodoAndProjectsRepository _todoAndProjectsRepository;
        private TodoAndUsersRepository _todoAndUsersRepository;
        private UsersRepository _usersRepository;

        private bool _disposed;

        public EfUnitOfWork(string connectionString)
        {
            _databaseContext = new TaskListContext(connectionString);
        }

        public Database Database => _databaseContext.Database;

        public IRepository<PriorityType> PriorityTypes => _priorityRepository ?? (_priorityRepository = new PriorityRepository(_databaseContext));
        public IRepository<RolesType> RolesTypes => _rolesRepository ?? (_rolesRepository = new RolesRepository(_databaseContext));
        public IRepository<ProjectInfo> ProjectInfo => _teamInfoRepository ?? (_teamInfoRepository = new ProjectInfoRepository(_databaseContext));
        public IRepository<Projects> Projects => _teamsRepository ?? (_teamsRepository = new ProjectsRepository(_databaseContext));
        public IRepository<Todo> Todos => _todosRepository ?? (_todosRepository = new TodosRepository(_databaseContext));
        public IRepository<TodoAndProjects> TodoAndProjects => _todoAndProjectsRepository ?? (_todoAndProjectsRepository = new TodoAndProjectsRepository(_databaseContext));
        public IRepository<TodoAndUsers> TodoAndUsers => _todoAndUsersRepository ?? (_todoAndUsersRepository = new TodoAndUsersRepository(_databaseContext));
        public IRepository<User> Users => _usersRepository ?? (_usersRepository = new UsersRepository(_databaseContext));

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _databaseContext.Dispose();
            }
            _disposed = true;
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
