using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;
using System.Data.Entity;

namespace TaskList.DAL.Repositories
{
    public class UsersRepository : IRepository<User>
    {
        private readonly TaskListContext _databaseContext;

        public UsersRepository(TaskListContext taskListContext)
        {
            _databaseContext = taskListContext;
        }

        public void Create(User item)
        {
            _databaseContext.Users.Add(item);
        }

        public void Delete(int id)
        {
            var item = _databaseContext.Users.Find(id);
            if(item != null)
            {
                _databaseContext.Users.Remove(item);
            }
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return _databaseContext.Users.Where(predicate).ToList();
        }

        public User Get(int id)
        {
            return _databaseContext.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _databaseContext.Users.Include(o => o.Role);
        }

        public void Update(User item)
        {
            _databaseContext.Entry(item).State = EntityState.Modified;
        }
    }
}
