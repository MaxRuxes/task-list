using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class RolesRepository : IRepository<RolesType>
    {
        private TaskListContext db;

        public RolesRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(RolesType item)
        {
            db.RolesTypes.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.RolesTypes.Find(id);
            if (item == null)
            {
                db.RolesTypes.Remove(item);
            }
        }

        public IEnumerable<RolesType> Find(Func<RolesType, bool> predicate)
        {
            return db.RolesTypes.Where(predicate).ToList();
        }

        public RolesType Get(int id)
        {
            return db.RolesTypes.Find(id);
        }

        public IEnumerable<RolesType> GetAll()
        {
            return db.RolesTypes;
        }

        public void Update(RolesType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
