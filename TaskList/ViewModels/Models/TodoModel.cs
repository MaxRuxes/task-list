using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskList.BLL.DTO;

namespace TaskList.ViewModels.Models
{
    public class TodoModel
    {
        public int TodoId { get; set; }
        public string Caption { get; set; }
        public string Content { get; set; }
        public int EstimatedHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndRealDate { get; set; }
        public int SpentTime { get; set; }
        public int State { get; set; }
        public UserDTO Owner { get; set; }
        public int IdPriority { get; set; }
        public PriorityModel Priority { get; set; }

        public string StateString { get; set; }

        public TodoModel Copy()
        {
            return new TodoModel()
            {
                TodoId = TodoId,
                Owner = Owner,
                Priority = Priority,
                Content = Content,
                Caption = Caption,
                EstimatedHours = EstimatedHours,
                StartDate = StartDate,
                EndRealDate = EndRealDate,
                SpentTime = SpentTime,
                State = State,
                IdPriority = IdPriority,
                StateString = StateString
            };
        }

    }
}
