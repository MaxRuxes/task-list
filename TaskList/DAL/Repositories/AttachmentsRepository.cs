using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class AttachmentsRepository : IRepository<Attachments>
    {
        private TaskListContext db;

        public AttachmentsRepository(TaskListContext context)
        {
            db = context;
        }

        public void Create(Attachments item)
        {
            db.Attachments.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.Attachments.Find(id);
            if (item != null)
            {
                db.Attachments.Remove(item);
            }
        }

        public IEnumerable<Attachments> Find(Func<Attachments, bool> predicate)
        {
            return db.Attachments.Where(predicate).ToList();
        }

        public Attachments Get(int id)
        {
            return db.Attachments.Find(id);
        }

        public IEnumerable<Attachments> GetAll()
        {
            return db.Attachments.Include(o => o.AttachType);
        }

        public void Update(Attachments item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
