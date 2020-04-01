using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class ProjectInfoRepository : IRepository<ProjectInfo>
    {
        private TaskListContext _databaseContext;

        public ProjectInfoRepository(TaskListContext taskListContext)
        {
            _databaseContext = taskListContext;
        }

        public ProjectInfo Create(ProjectInfo item)
        {
            return _databaseContext.ProjectInfo.Add(item);
        }

        public void Delete(int id)
        {
            var item = _databaseContext.ProjectInfo.Find(id);
            if (item != null)
            {
                _databaseContext.ProjectInfo.Remove(item);
            }
        }

        public IEnumerable<ProjectInfo> Find(Func<ProjectInfo, bool> predicate)
        {
            return _databaseContext.ProjectInfo.Where(predicate).ToList();
        }

        public ProjectInfo Get(int id)
        {
            return _databaseContext.ProjectInfo.Find(id);
        }

        public IEnumerable<ProjectInfo> GetAll()
        {
            return _databaseContext.ProjectInfo;
        }

        public void Update(ProjectInfo item)
        {
            _databaseContext.ProjectInfo.AddOrUpdate(item);
        }
    }
}
