using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;
using TaskList.Model;
using TaskList.View;

namespace TaskList.ViewModel
{
    public class UrgentUnimportantTasksVM : ObservableObject
    {
        private string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TaskList";
        private string _fileName = @"\UrgentUnimportantTasks.xml", _fileDeletesName = @"\UrgentUnimportantTasks(Deletes).txt";
        private ObservableCollection<TheTask> _ms, _items = new ObservableCollection<TheTask>();

        [XmlArray("Collection"), XmlArrayItem("Item")]
        public ObservableCollection<TheTask> TaskCollection
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChangedEvent(nameof(TaskCollection));
            }
        }

        public bool Enabled { get { return CurrentTask != null; } }

        private TheTask _current = null;
        public TheTask CurrentTask
        {
            get { return _current; }
            set
            {
                _current = value;
                RaisePropertyChangedEvent(nameof(CurrentTask));
                RaisePropertyChangedEvent(nameof(Enabled));
            }
        }

        public UrgentUnimportantTasksVM()
        {
            LoadTaskFromFile();
            RaisePropertyChangedEvent(nameof(TaskCollection));
        }

        ~UrgentUnimportantTasksVM()
        {
            SerializeAndWriteCollection(_items);
        }

        public ICommand AddTask
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    TaskWindow temp = new TaskWindow();
                    TaskWindowViewModel twvm = (TaskWindowViewModel)temp.DataContext;

                    if (temp.ShowDialog() == true)
                    {
                        if (twvm.Content.Trim() != "")
                        {
                            File.AppendAllText(_filePath + @"\logs.txt", new string('.', 25) + "\tНЕВАЖНОЕ И СРОЧНОЕ ДЕЛО" + new string('.', 25) + Environment.NewLine);
                            _items.Add(new TheTask(twvm.Content));
                            File.AppendAllText(_filePath + @"\logs.txt", new string('.', 80) + Environment.NewLine);
                        }
                    }
                    RaisePropertyChangedEvent(nameof(TaskCollection));
                });
            }
        }

        public ICommand CompleteTask
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    int i = _items.IndexOf(CurrentTask);
                    if (i != -1)
                    {
                        if (CurrentTask.Status == "Выполнено") return;
                        _items[i].SetStatus(1);
                        UpdateCollection();
                        CurrentTask = _items[i];
                    }
                    RaisePropertyChangedEvent(nameof(TaskCollection));
                });
            }
        }

        public ICommand DeleteTask
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    int i = _items.IndexOf(CurrentTask);
                    if (i != -1)
                    {
                        File.AppendAllText(_filePath + _fileDeletesName, String.Format(DateTime.Now + "\tДело \"{0}\" со статусом \"{1}\", начатое {2} удалено из списка." + Environment.NewLine, _items[i].Content, _items[i].Status, _items[i].StartDate));
                        _items[i].SetStatus(2);
                        _items[i].DeleteTask();
                        _items.RemoveAt(i);
                    }
                });
            }
        }

        public ICommand EditTask
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    TaskWindow temp = new TaskWindow();
                    TaskWindowViewModel twvm = (TaskWindowViewModel)temp.DataContext;
                    twvm.Content = CurrentTask.Content;

                    if (temp.ShowDialog() == true)
                    {
                        if (twvm.Content.Trim() != "")
                        {
                            int i = _items.IndexOf(CurrentTask);
                            if (i != -1)
                            {
                                _items[i].SetContent(twvm.Content);
                                UpdateCollection();
                                CurrentTask = _items[i];
                            }
                        }
                    }
                    RaisePropertyChangedEvent(nameof(TaskCollection));
                });
            }
        }

        private void UpdateCollection()
        {
            _ms = new ObservableCollection<TheTask>();
            _items.ToList().ForEach((temp) => _ms.Add(temp));
            _items.Clear();
            _ms.ToList().ForEach((temp) => _items.Add(temp));
        }

        private void LoadTaskFromFile()
        {
            if (!Directory.Exists(_filePath)) { Directory.CreateDirectory(_filePath); return; }
            if (!File.Exists(_filePath + _fileName)) { return; }
            if (!File.Exists(_filePath + _fileDeletesName)) { File.Create(_filePath + _fileDeletesName); }
            ReadAndDeserializeCollection(ref _items);
        }

        #region (De)SerializeCollection
        private void SerializeAndWriteCollection(ObservableCollection<TheTask> serializeCollection)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<TheTask>));
            TextWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, serializeCollection);
            File.WriteAllText(_filePath + _fileName, stringWriter.ToString());
        }

        private void ReadAndDeserializeCollection(ref ObservableCollection<TheTask> deserializeCollection)
        {
            string serializedData = File.ReadAllText(_filePath + _fileName);
            var xmlSerializer = new XmlSerializer(typeof(ObservableCollection<TheTask>));
            var stringReader = new StringReader(serializedData);
            deserializeCollection = (ObservableCollection<TheTask>)xmlSerializer.Deserialize(stringReader);
        }
        #endregion
    }
}
