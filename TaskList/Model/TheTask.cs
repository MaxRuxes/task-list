using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TaskList.Model
{
    public class TheTask
    {
        private string _content;
        private string _status;
        private DateTime _startDate;
        private DateTime _endDate;

        public string Content { get { return _content; } }
        public string Status { get { return _status; } }
        public string StartDate { get { return _startDate.ToShortDateString(); } }
        public string EndDate { get { return _endDate.ToShortDateString(); } }

        public TheTask()
        {
            _startDate = DateTime.Now.Date;
            _content = "1";
        }

        public void SetContent(string content)
        {
            _content = content;
        }

        public void SetStatus(string status)
        {
            _status = status;
        }

        public void CloseTask()
        {
            _endDate = DateTime.Now.Date;
        }

        public override string ToString()
        {
            return this.Content;
        }

    }
}
