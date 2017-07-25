using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskList.Model;

namespace TaskList.ViewModel
{
    public class NonUrgentImportantTasksVM : ObservableObject
    {
        private ObservableCollection<TheTask> _tasks = new ObservableCollection<TheTask>();
        public ObservableCollection<TheTask> TaskCollection { get { return _tasks; } set { _tasks = value; } }
        private TheTask _current = null;
        public TheTask CurrentTask { get { return _current; } set { _current = value; } }

        public ICommand AddTask
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public  ICommand CompleteTask
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
