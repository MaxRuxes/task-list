using System;

namespace TaskList.Models
{
    public class TodoModel 
    {
        public int TodoId { get; set; }
        public int IdPriority { get; set; }
        public PriorityModel Priority { get; set; }
        public string ContentTodo { get; set; }
        public int EstimatedHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndRealDate { get; set; }
    }
}
