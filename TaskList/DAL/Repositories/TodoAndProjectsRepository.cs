using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Entities;
using TaskList.DAL.Interfaces;

namespace TaskList.DAL.Repositories
{
    public class TodoAndProjectsRepository : IRepository<TodoAndProjects>
    {
        private readonly TaskListContext _context;

        public TodoAndProjectsRepository(TaskListContext taskListContext)
        {
            _context = taskListContext;
        }

        public TodoAndProjects Create(TodoAndProjects item)
        {
            return _context.TodoAndProjects.Add(item);
        }

        public void Delete(int id)
        {
            var item = _context.TodoAndProjects.Find(id);
            if (item != null)
            {
                _context.TodoAndProjects.Remove(item);
            }
        }

        public IEnumerable<TodoAndProjects> Find(Func<TodoAndProjects, bool> predicate)
        {
            return _context
                .TodoAndProjects
                .Where(predicate)
                .ToList();
        }

        public TodoAndProjects Get(int id)
        {
            return _context
                .TodoAndProjects
                .Find(id);
        }

        public IEnumerable<TodoAndProjects> GetAll()
        {
            return _context
                .TodoAndProjects
                .Include(o => o.Project)
                .Include(o => o.Todo);
        }

        public int GetCountForProject(int id)
        {
            return _context.TodoAndProjects.Count(x => x.IdProject == id);
        }

        public void Update(TodoAndProjects item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
