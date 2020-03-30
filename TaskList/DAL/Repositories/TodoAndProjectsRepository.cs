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
        private readonly TaskListContext _databaseContext;

        public TodoAndProjectsRepository(TaskListContext taskListContext)
        {
            _databaseContext = taskListContext;
        }

        public TodoAndProjects Create(TodoAndProjects item)
        {
            return _databaseContext.TodoAndProjects.Add(item);
        }

        public void Delete(int id)
        {
            var item = _databaseContext.TodoAndProjects.Find(id);
            if (item != null)
            {
                _databaseContext.TodoAndProjects.Remove(item);
            }
        }

        public IEnumerable<TodoAndProjects> Find(Func<TodoAndProjects, bool> predicate)
        {
            return _databaseContext
                .TodoAndProjects
                .Where(predicate)
                .ToList();
        }

        public TodoAndProjects Get(int id)
        {
            return _databaseContext
                .TodoAndProjects
                .Find(id);
        }

        public IEnumerable<TodoAndProjects> GetAll()
        {
            return _databaseContext
                .TodoAndProjects
                .Include(o => o.Project)
                .Include(o => o.Todo);
        }

        public void Update(TodoAndProjects item)
        {
            _databaseContext.Entry(item).State = EntityState.Modified;
        }
    }
}
