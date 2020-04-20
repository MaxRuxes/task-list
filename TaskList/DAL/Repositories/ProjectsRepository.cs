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
        private readonly TaskListContext _context;

        public ProjectsRepository(TaskListContext taskListContext)
        {
            _context = taskListContext;
        }

        public Projects Create(Projects item)
        {
            return _context.Projects.Add(item);
        }

        public void Delete(int id)
        {
            var item = _context.Projects.Find(id);
            if (item != null)
            {
                _context.Projects.Remove(item);
            }
        }

        public IEnumerable<Projects> Find(Func<Projects, bool> predicate)
        {
            return _context.Projects.Where(predicate).ToList();
        }

        public Projects Get(int id)
        {
            return _context.Projects.Find(id);
        }

        public IEnumerable<Projects> GetAll()
        {
            return _context
                .Projects
                .Include(o => o.ProjectInfo)
                .Include(o => o.User);
        }

        public void Update(Projects item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
