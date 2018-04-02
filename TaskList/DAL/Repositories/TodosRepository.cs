using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace TaskList.DAL.Repositories
{
    public class TodosRepository : IRepository<Todo>
    {
        private TaskListContext db;

        public TodosRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(Todo item)
        {
            db.Todos.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Todos.Find(id);
            if (item != null)
            {
                db.Todos.Remove(item);
            }
        }

        public IEnumerable<Todo> Find(Func<Todo, bool> predicate)
        {
            return db.Todos.Where(predicate).ToList();
        }

        public Todo Get(int id)
        {
            return db.Todos.Find(id);
        }

        public IEnumerable<Todo> GetAll()
        {
            return db.Todos.Include(par => par.Priority);
        }

        public void Update(Todo item)
        {
            db.Todos.AddOrUpdate(item);
        }
    }
}
