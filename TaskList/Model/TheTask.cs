using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace TaskList.Model
{
    [XmlType("Task")]
    public class TheTask
    {
        private DateTime _startDate, _endDate;
        private string _status;
        private readonly string _fileLogPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TaskList\logs.txt";

        [XmlAttribute("ID")]
        public int Id { get; set; }

        public string Content { get; set; }
        public string Status
        {
            get => _status;
            set => _status = value;
        }
        public string StartDate
        {
            get => _startDate.ToString(CultureInfo.CurrentCulture);
            set => _startDate = Convert.ToDateTime(value);
        }
        public string EndDate
        {
            get => _endDate.ToString(CultureInfo.InvariantCulture);
            set => _endDate = Convert.ToDateTime(value);
        }

        //создано новое дело
        public TheTask()
        {
            _startDate = DateTime.Now;
            Content = "В процессе";
        }

        public TheTask(string content)
        {
            _startDate = DateTime.Now;
            Content = content;
            Content = "В процессе";
            File.AppendAllText(_fileLogPath, String.Format(DateTime.Now + "\tСоздано новое дело с содержанием: \"{0}\" со статусом: {1}" + Environment.NewLine, Content, _status));
        }

        //изменено содержание
        public void SetContent(string content)
        {
            string old = Content;
            Content = content;
            File.AppendAllText(_fileLogPath, String.Format(DateTime.Now + "\tСодержание дела \"{0}\" изменено на \"{1}\"" + Environment.NewLine, old, Content));
        }
        
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
            File.AppendAllText(_fileLogPath, string.Format(DateTime.Now + "\tСтатус дела \"{0}\" изменен на \"{1}\"" + Environment.NewLine, Content, _status));
            if (status == 1 || status == 2) CloseTask();
        }

        // дело закрыто
        private void CloseTask()
        {
            _endDate = DateTime.Now;
            File.AppendAllText(_fileLogPath, string.Format(DateTime.Now + "\tДело закрыто со статусом \"{0}\"" + Environment.NewLine, _status));
        }

        public void DeleteTask()
        {
            File.AppendAllText(_fileLogPath, string.Format(DateTime.Now + "\tДело \"{0}\" удалено из списка" + Environment.NewLine, Content));
        }
    }
}
