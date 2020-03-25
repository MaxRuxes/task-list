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
        private readonly TaskListContext _databaseContext;

        public TodosRepository(TaskListContext taskListContext)
        {
            _databaseContext = taskListContext;
        }

        public void Create(Todo item)
        {
            _databaseContext.Todos.Add(item);
        }

        public void Delete(int id)
        {
            var item = _databaseContext.Todos.Find(id);
            if (item != null)
            {
                _databaseContext.Todos.Remove(item);
            }
        }

        public IEnumerable<Todo> Find(Func<Todo, bool> predicate)
        {
            return _databaseContext.Todos.Where(predicate).ToList();
        }

        public Todo Get(int id)
        {
            return _databaseContext.Todos.Find(id);
        }

        public IEnumerable<Todo> GetAll()
        {
            return _databaseContext.Todos.Include(par => par.Priority);
        }

        public void Update(Todo item)
        {
            _databaseContext.Todos.AddOrUpdate(item);
        }
    }
}
