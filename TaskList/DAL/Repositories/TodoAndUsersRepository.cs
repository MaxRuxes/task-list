using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using System.Data.Entity;

namespace TaskList.DAL.Repositories
{
    public class TodoAndUsersRepository : IRepository<TodoAndUsers>
    {
        private TaskListContext db;

        public TodoAndUsersRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(TodoAndUsers item)
        {
            db.TodoAndusers.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.TodoAndusers.Find(id);
            if(item != null)
            {
                db.TodoAndusers.Remove(item);
            }
        }

        public IEnumerable<TodoAndUsers> Find(Func<TodoAndUsers, bool> predicate)
        {
            return db.TodoAndusers.Where(predicate).ToList();
        }

        public TodoAndUsers Get(int id)
        {
            return db.TodoAndusers.Find(id);
        }

        public IEnumerable<TodoAndUsers> GetAll()
        {
            return db.TodoAndusers.Include(o => o.User).Include(o => o.Todo);
        }

        public void Update(TodoAndUsers item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
