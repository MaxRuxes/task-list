using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TaskList.Model;
using TaskList.View;

namespace TaskList.ViewModel
{
    public class NonUrgentImportantTasksVM : ObservableObject
    {
        private ObservableCollection<TheTask> _ms, _items = new ObservableCollection<TheTask>();
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
                            _items.Add(new TheTask(twvm.Content));
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
                            _items[i].SetContent((_items[i].Content +' '+ _items[i].Status));

                            _items.ToList<TheTask>().ForEach((temp) => _ms.Add(temp));
                            _items.Clear();
                            _ms.ToList<TheTask>().ForEach((temp) => _items.Add(temp));
                            CurrentTask = _items[i];
                            break;
                        }
                    }
                    RaisePropertyChangedEvent(nameof(TaskCollection));
                });
            }
        }
    }
}
