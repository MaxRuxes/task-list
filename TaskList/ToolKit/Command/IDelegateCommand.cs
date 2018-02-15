using System.Windows.Input;

namespace TaskList.ToolKit.Command
{
    public interface IDelegateCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
