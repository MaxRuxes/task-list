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
        private readonly TaskListContext _context;

        public TodoAndUsersRepository(TaskListContext taskListContext)
        {
            _context = taskListContext;
        }

        public TodoAndUsers Create(TodoAndUsers item)
        {
            return _context.TodoAndUsers.Add(item);
        }

        public void Delete(int id)
        {
            var item = _context.TodoAndUsers.Find(id);
            if(item != null)
            {
                _context.TodoAndUsers.Remove(item);
            }
        }

        public IEnumerable<TodoAndUsers> Find(Func<TodoAndUsers, bool> predicate)
        {
            return _context
                .TodoAndUsers
                .Where(predicate)
                .ToList();
        }

        public TodoAndUsers Get(int id)
        {
            return _context.TodoAndUsers.Find(id);
        }

        public IEnumerable<TodoAndUsers> GetAll()
        {
            return _context
                .TodoAndUsers
                .Include(o => o.User)
                .Include(o => o.Todo);
        }

        public void Update(TodoAndUsers item)
        {
            _context.TodoAndUsers.AddOrUpdate(item);
        }
    }
}
