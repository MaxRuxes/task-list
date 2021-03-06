﻿using System;

namespace TaskList.BLL.DTO
{
    public class TodoDTO
    {
        public int TodoId { get; set; }
        public int IdPriority { get; set; }
        public string Caption { get; set; }
        public string Content { get; set; }
        public int EstimatedHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndRealDate { get; set; }
        public int SpentTime { get; set; }
        public int State { get; set; }
        public PriorityTypeDTO Priority { get; set; }
        }
}
