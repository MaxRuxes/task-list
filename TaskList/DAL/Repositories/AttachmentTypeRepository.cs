using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class AttachmentTypeRepository : IRepository<AttachmentType>
    {
        private TaskListContext db;

        public AttachmentTypeRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(AttachmentType item)
        {
            db.AttachmentTypes.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.AttachmentTypes.Find(id);
            if (item != null)
            {
                db.AttachmentTypes.Remove(item);
            }
        }

        public IEnumerable<AttachmentType> Find(Func<AttachmentType, bool> predicate)
        {
            return db.AttachmentTypes.Where(predicate).ToList();
        }

        public AttachmentType Get(int id)
        {
            return db.AttachmentTypes.Find(id);
        }

        public IEnumerable<AttachmentType> GetAll()
        {
            return db.AttachmentTypes;
        }

        public void Update(AttachmentType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
