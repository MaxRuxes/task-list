using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.DAL.Repositories
{
    public class TeamInfoRepository : IRepository<TeamInfo>
    {
        private TaskListContext db;

        public TeamInfoRepository(TaskListContext taskListContext)
        {
            db = taskListContext;
        }

        public void Create(TeamInfo item)
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

        public IEnumerable<TeamInfo> Find(Func<TeamInfo, bool> predicate)
        {
            return db.TeamsInfo.Where(predicate).ToList();
        }

        public TeamInfo Get(int id)
        {
            return db.TeamsInfo.Find(id);
        }

        public IEnumerable<TeamInfo> GetAll()
        {
            return db.TeamsInfo;
        }

        public void Update(TeamInfo item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
