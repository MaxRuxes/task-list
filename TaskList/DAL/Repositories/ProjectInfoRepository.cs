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

        public void Create(ProjectInfo item)
        {
            db.TeamsInfo.Add(item);
        }

        public void Delete(int id)
        {
            var item = db.TeamsInfo.Find(id);
            if (item != null)
            {
                db.TeamsInfo.Remove(item);
            }
        }

        public IEnumerable<ProjectInfo> Find(Func<ProjectInfo, bool> predicate)
        {
            return db.TeamsInfo.Where(predicate).ToList();
        }

        public ProjectInfo Get(int id)
        {
            return db.TeamsInfo.Find(id);
        }

        public IEnumerable<ProjectInfo> GetAll()
        {
            return db.TeamsInfo;
        }

        public void Update(ProjectInfo item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
