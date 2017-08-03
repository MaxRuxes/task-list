using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace TaskList.Model
{
    [Serializable]
    public class TheTask
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private string _content, _status;
        private string _fileLogPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TaskList\logs.txt";

        public string Content { get { return _content; } set { _content = value; } }
        public string Status { get { return _status; } set { _status = value; } }
        public string StartDate { get { return _startDate.ToString(); } set { _startDate = Convert.ToDateTime(value); } }
        public string EndDate { get { return _endDate.ToString(); } set { _endDate = Convert.ToDateTime(value); } }

        //создано новое дело
        public TheTask()
        {
            _startDate = DateTime.Now;
            _status = "В процессе";
        }

        public TheTask(string content)
        {
            _startDate = DateTime.Now;
            _content = content;
            _status = "В процессе";
            File.AppendAllText(_fileLogPath, String.Format(DateTime.Now + "\tСоздано новое дело с содержанием: \"{0}\" со статусом: {1}" + Environment.NewLine, _content, _status));
        }

        //изменено содержание
        public void SetContent(string content)
        {
            string old = _content;
            _content = content;
            File.AppendAllText(_fileLogPath, String.Format(DateTime.Now + "\tСодержание дела \"{0}\" изменено на \"{1}\"" + Environment.NewLine, old, _content));
        }

        //измнен статус
        /// <summary>
        /// Установить статус выполнения дела:
        /// 0 -- В процессе;
        /// 1 -- Выполнено;
        /// 2 -- Удалено.
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(int status)
        {
            switch (status)
            {
                case 1: _status = "Выполнено"; break;
                case 2: _status = "Удалено"; break;
                default: _status = "В процессе"; break;
            }
            File.AppendAllText(_fileLogPath, String.Format(DateTime.Now + "\tСтатус дела \"{0}\" изменен на \"{1}\"" + Environment.NewLine, _content, _status));
            if (status == 1 || status == 2) CloseTask();
        }

        // дело закрыто
        private void CloseTask()
        {
            _endDate = DateTime.Now;
            File.AppendAllText(_fileLogPath, String.Format(DateTime.Now + "\tДело закрыто со статусом \"{0}\"" + Environment.NewLine, _status));
        }

        public void DeleteTask()
        {
            File.AppendAllText(_fileLogPath, String.Format(DateTime.Now + "\tДело \"{0}\" удалено из списка" + Environment.NewLine, _content));
        }
    }
}
