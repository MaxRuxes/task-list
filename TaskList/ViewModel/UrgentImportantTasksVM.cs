using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TaskList.Model;
using TaskList.View;

namespace TaskList.ViewModel
{
    public class UrgentImportantTasksVM : ObservableObject
    {
        private string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\TaskList";
        private string _fileName = @"\UrgentAndImportant.xml";

        private ObservableCollection<TheTask> _ms, _items = new ObservableCollection<TheTask>();

        [XmlArray("Collection"), XmlArrayItem("Item")]
        public ObservableCollection<TheTask> TaskCollection
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChangedEvent(nameof(TaskCollection));
                }

            }
        }

        private TheTask _current = null;
        public TheTask CurrentTask
        {
            get { return _current; }
            set
            {
                _current = value;
                RaisePropertyChangedEvent(nameof(CurrentTask));
            }
        }

        public UrgentImportantTasksVM()
        {
            LoadTaskFromFile();
            RaisePropertyChangedEvent(nameof(TaskCollection));
        }

        ~UrgentImportantTasksVM()
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
                            File.AppendAllText(_filePath+@"\logs.txt", DateTime.Now + "\tВАЖНОЕ И СРОЧНОЕ ДЕЛО" + Environment.NewLine);
                            _items.Add(new TheTask(twvm.Content));
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
                    for (int i = 0; i < TaskCollection.Count; i++)
                    {
                        if (CurrentTask == null) return;
                        _ms = new ObservableCollection<TheTask>();
                        if (_items[i].Content == CurrentTask.Content)
                        {
                            _items[i].SetStatus(1);
                            _items.ToList().ForEach((temp) => _ms.Add(temp));
                            _items.Clear();
                            _ms.ToList().ForEach((temp) => _items.Add(temp));
                            CurrentTask = _items[i];
                            break;
                        }
                    }
                    RaisePropertyChangedEvent(nameof(TaskCollection));
                });
            }
        }

        private void LoadTaskFromFile()
        {
            if (!Directory.Exists(_filePath)) { Directory.CreateDirectory(_filePath); return; }
            if (!File.Exists(_filePath + _fileName)) { return; }
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
