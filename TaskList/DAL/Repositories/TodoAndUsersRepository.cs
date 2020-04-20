using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace TaskList.DAL.Repositories
{
    public class TodoAndUsersRepository : IRepository<TodoAndUsers>
    {
        private readonly TaskListContext _databaseContext;

        public TodoAndUsersRepository(TaskListContext taskListContext)
        {
            _databaseContext = taskListContext;
        }

        public TodoAndUsers Create(TodoAndUsers item)
        {
            return _databaseContext.TodoAndUsers.Add(item);
        }

        public void Delete(int id)
        {
            var item = _databaseContext.TodoAndUsers.Find(id);
            if(item != null)
            {
                _databaseContext.TodoAndUsers.Remove(item);
            }
        }

        public IEnumerable<TodoAndUsers> Find(Func<TodoAndUsers, bool> predicate)
        {
            return _databaseContext
                .TodoAndUsers
                .Where(predicate)
                .ToList();
        }

        public TodoAndUsers Get(int id)
        {
            return _databaseContext.TodoAndUsers.Find(id);
        }

        public IEnumerable<TodoAndUsers> GetAll()
        {
            return _databaseContext
                .TodoAndUsers
                .Include(o => o.User)
                .Include(o => o.Todo);
        }

        public void Update(TodoAndUsers item)
        {
            _databaseContext.TodoAndUsers.AddOrUpdate(item);
        }
    }
}
