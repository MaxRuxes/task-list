using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;
using System.Data.Entity.Migrations;

namespace TaskList.DAL.Repositories
{
    public class UsersRepository : IRepository<User>
    {
        private readonly TaskListContext _context;

        public UsersRepository(TaskListContext taskListContext)
        {
            _context = taskListContext;
        }

        public User Create(User item)
        {
            return _context.Users.Add(item);
        }

        public void Delete(int id)
        {
            var item = _context.Users.Find(id);
            if(item != null)
            {
                _context.Users.Remove(item);
            }
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return _context.Users.Where(predicate).ToList();
        }

        public User Get(int id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public void Update(User item)
        {
            _context.Users.AddOrUpdate(item);
        }
    }
}
