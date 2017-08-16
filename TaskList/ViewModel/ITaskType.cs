using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using TaskList.Model;

namespace TaskList.ViewModel
{
    public interface ITaskType
    {
        // Fields
        ObservableCollection<TheTask> TaskCollection { get; set; }
        TheTask CurrentTask { get; set; }

        // Commands
        ICommand AddTask { get; }
        ICommand CompleteTask { get; }
        ICommand EditTask { get; }
        ICommand DeleteTask { get; }
    }
}
