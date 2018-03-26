using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;
using System.Data.Entity;

namespace TaskList.DAL.Repositories
{
    public class UsersRepository : IRepository<User>
    {
        private TaskListContext db;

        public UsersRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(User item)
        {
            db.Users.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Users.Find(id);
            if(item != null)
            {
                db.Users.Remove(item);
            }
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users.Include(o => o.Role);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
