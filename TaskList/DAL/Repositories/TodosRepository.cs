using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace TaskList.DAL.Repositories
{
    public class TodosRepository : IRepository<Todo>
    {
        private readonly TaskListContext _context;

        public TodosRepository(TaskListContext taskListContext)
        {
            _context = taskListContext;
        }

        public Todo Create(Todo item)
        {
            return _context.Todos.Add(item);
        }

        public void Delete(int id)
        {
            var item = _context.Todos.Find(id);
            if (item != null)
            {
                _context.Todos.Remove(item);
            }
        }

        public IEnumerable<Todo> Find(Func<Todo, bool> predicate)
        {
            return _context.Todos.Where(predicate).ToList();
        }

        public Todo Get(int id)
        {
            return _context.Todos.Find(id);
        }

        public IEnumerable<Todo> GetAll()
        {
            return _context.Todos.Include(par => par.Priority);
        }

        public void Update(Todo item)
        {
            _context.Todos.AddOrUpdate(item);
        }
    }
}
