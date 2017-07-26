using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskList.Model;

namespace TaskList.ViewModel
{
    public class NonUrgentUnimportantTasksVM
    {
        private ObservableCollection<TheTask> _tasks = new ObservableCollection<TheTask>();
        public ObservableCollection<TheTask> TaskCollection { get { return _tasks; } set { _tasks = value; } }
        private TheTask _current = null;
        public TheTask CurrentTask { get { return _current; } set { _current = value; } }

        public ICommand AddTask
        {
            get
            {
                return new DelegateCommand((o) => { TaskCollection.Add(new TheTask()); });
            }
        }

        public  ICommand CompleteTask
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    for (int i = 0; i < TaskCollection.Count; i++)
                    {
                        if(TaskCollection[i].Content == CurrentTask.ToString())
                        {
                            TaskCollection[i].CloseTask();
                            TaskCollection[i].SetContent(TaskCollection[i].Content + " (complete) ");
                            break;
                        }
                    }
                });
            }
        }

        
    }
}
