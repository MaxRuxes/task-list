using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class TeamsRepository : IRepository<Teams>
    {
        private TaskListContext db;

        public TeamsRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(Teams item)
        {
            db.Teams.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Teams.Find(id);
            if (item != null)
            {
                db.Teams.Remove(item);
            }
        }

        public IEnumerable<Teams> Find(Func<Teams, bool> predicate)
        {
            return db.Teams.Where(predicate).ToList();
        }

        public Teams Get(int id)
        {
            return db.Teams.Find(id);
        }

        public IEnumerable<Teams> GetAll()
        {
            return db.Teams.Include(o => o.TeamInfo).Include(o => o.User);
        }

        public void Update(Teams item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
