using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class ProjectsRepository : IRepository<Projects>
    {
        private readonly TaskListContext _databaseContext;

        public ProjectsRepository(TaskListContext taskListContext)
        {
            _databaseContext = taskListContext;
        }

        public void Create(Projects item)
        {
            _databaseContext.Projects.Add(item);
        }

        public void Delete(int id)
        {
            var item = _databaseContext.Projects.Find(id);
            if (item != null)
            {
                _databaseContext.Projects.Remove(item);
            }
        }

        public IEnumerable<Projects> Find(Func<Projects, bool> predicate)
        {
            return _databaseContext.Projects.Where(predicate).ToList();
        }

        public Projects Get(int id)
        {
            return _databaseContext.Projects.Find(id);
        }

        public IEnumerable<Projects> GetAll()
        {
            return _databaseContext
                .Projects
                .Include(o => o.ProjectInfo)
                .Include(o => o.User);
        }

        public void Update(Projects item)
        {
            _databaseContext.Entry(item).State = EntityState.Modified;
        }
    }
}
