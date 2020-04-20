using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.DAL.Repositories
{
    public class PriorityRepository : IRepository<PriorityType>
    {
        private readonly TaskListContext _context;

        public PriorityRepository(TaskListContext taskListContext)
        {
            _context = taskListContext;
        }

        public PriorityType Create(PriorityType item)
        {
            return _context.PriorityTypes.Add(item);
        }

        public void Delete(int id)
        {
            var item = _context.PriorityTypes.Find(id);
            if(item != null)
            {
                _context.PriorityTypes.Remove(item);
            }
        }

        public IEnumerable<PriorityType> Find(Func<PriorityType, bool> predicate)
        {
            return _context.PriorityTypes.Where(predicate).ToList();
        }

        public PriorityType Get(int id)
        {
            return _context.PriorityTypes.Find(id);
        }

        public IEnumerable<PriorityType> GetAll()
        {
            return _context.PriorityTypes;
        }

        public void Update(PriorityType item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
