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
        private string _content;
        private string _status;
        private DateTime _startDate;
        private DateTime _endDate;
        private enum StatusCase { InProcess, Complete, Deleted }
        private string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TaskList\logs.txt";

        public string Content { get { return _content; } set { _content = value; } }
        public string Status { get { return _status; } set { _status = value; } }
        public string StartDate { get { return _startDate.ToShortDateString(); } set { _startDate = Convert.ToDateTime(value); } }
        public string EndDate { get { return _endDate.ToShortDateString(); } set { _endDate = Convert.ToDateTime(value); } }

        //создано новое дело
        public TheTask()
        {
            _startDate = DateTime.Now.Date;
            _status = "В процессе";
        }

        public TheTask(string content)
        {
            _startDate = DateTime.Now.Date;
            _content = content;
            _status = "В процессе";
            File.AppendAllText(_filePath, DateTime.Now + "\tСоздано новое дело с содержанием: \"" + this._content + "\" со статусом: " + this._status + Environment.NewLine);
        }

        //изменено содержание
        public void SetContent(string content)
        {
            string old = _content;
            _content = content;
            File.AppendAllText(_filePath, DateTime.Now + "\tСодержание дела \"" + old + "\" изменено на \"" + this._content + "\"" + Environment.NewLine);
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
            File.AppendAllText(_filePath, DateTime.Now + "\tСтатус дела \"" + this._content + "\" изменен на \"" + this._status + "\"" + Environment.NewLine);
            if (status == 1 || status == 2) CloseTask();
        }

        // дело закрыто
        private void CloseTask()
        {
            _endDate = DateTime.Now.Date;
            File.AppendAllText(_filePath, DateTime.Now + "\tДело закрыто со статусом \"" + this._status + "\"" + Environment.NewLine);
        }

        public void DeleteTask()
        {
            File.AppendAllText(_filePath, DateTime.Now + "\tДело \"" + this._content + "\" удалено из списка" + Environment.NewLine);
        }
    }
}
