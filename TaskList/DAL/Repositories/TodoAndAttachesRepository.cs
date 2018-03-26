using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using System.Data.Entity;

namespace TaskList.DAL.Repositories
{
    public class TodoAndAttachesRepository : IRepository<TodoAndAttaches>
    {
        TaskListContext db;

        public TodoAndAttachesRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(TodoAndAttaches item)
        {
            db.TodoAndAttachments.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.TodoAndAttachments.Find(id);
            if(item != null)
            {
                db.TodoAndAttachments.Remove(item);
            }
        }

        public IEnumerable<TodoAndAttaches> Find(Func<TodoAndAttaches, bool> predicate)
        {
            return db.TodoAndAttachments.Where(predicate).ToList();
        }

        public TodoAndAttaches Get(int id)
        {
            return db.TodoAndAttachments.Find(id);
        }

        public IEnumerable<TodoAndAttaches> GetAll()
        {
            return db.TodoAndAttachments.Include(o => o.Todo).Include(o => o.Attachments);
        }

        public void Update(TodoAndAttaches item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
