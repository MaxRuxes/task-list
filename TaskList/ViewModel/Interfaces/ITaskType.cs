using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskList.Model;

namespace TaskList.ViewModel.Interfaces
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
