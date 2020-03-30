using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class ProjectInfoRepository : IRepository<ProjectInfo>
    {
        private TaskListContext db;

        public ProjectInfoRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public ProjectInfo Create(ProjectInfo item)
        {
            return db.ProjectInfo.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.ProjectInfo.Find(id);
            if (item != null)
            {
                db.ProjectInfo.Remove(item);
            }
        }

        public IEnumerable<ProjectInfo> Find(Func<ProjectInfo, bool> predicate)
        {
            return db.ProjectInfo.Where(predicate).ToList();
        }

        public ProjectInfo Get(int id)
        {
            return db.ProjectInfo.Find(id);
        }

        public IEnumerable<ProjectInfo> GetAll()
        {
            return db.ProjectInfo;
        }

        public void Update(ProjectInfo item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
