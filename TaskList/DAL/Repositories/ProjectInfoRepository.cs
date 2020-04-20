using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class ProjectInfoRepository : IRepository<ProjectInfo>
    {
        private readonly TaskListContext _context;

        public ProjectInfoRepository(TaskListContext taskListContext)
        {
            _context = taskListContext;
        }

        public ProjectInfo Create(ProjectInfo item)
        {
            return _context.ProjectInfo.Add(item);
        }

        public void Delete(int id)
        {
            var item = _context.ProjectInfo.Find(id);
            if (item != null)
            {
                _context.ProjectInfo.Remove(item);
            }
        }

        public IEnumerable<ProjectInfo> Find(Func<ProjectInfo, bool> predicate)
        {
            return _context.ProjectInfo.Where(predicate).ToList();
        }

        public ProjectInfo Get(int id)
        {
            return _context.ProjectInfo.Find(id);
        }

        public IEnumerable<ProjectInfo> GetAll()
        {
            return _context.ProjectInfo;
        }

        public void Update(ProjectInfo item)
        {
            _context.ProjectInfo.AddOrUpdate(item);
        }
    }
}
