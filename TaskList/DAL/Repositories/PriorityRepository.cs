using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class PriorityRepository : IRepository<PriorityType>
    {
        private TaskListContext db;

        public PriorityRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(PriorityType item)
        {
            db.PriorityTypes.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.PriorityTypes.Find(id);
            if(item != null)
            {
                db.PriorityTypes.Remove(item);
            }
        }

        public IEnumerable<PriorityType> Find(Func<PriorityType, bool> predicate)
        {
            return db.PriorityTypes.Where(predicate).ToList();
        }

        public PriorityType Get(int id)
        {
            return db.PriorityTypes.Find(id);
        }

        public IEnumerable<PriorityType> GetAll()
        {
            return db.PriorityTypes;
        }

        public void Update(PriorityType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
