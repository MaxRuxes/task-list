using System;
using TaskList.ViewModel;

namespace TaskList.ToolKit.Command
{
    public class DelegateCommand : IDelegateCommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action<object> execute)
        {
            _execute = execute;
            _canExecute = AlwaysCanExecute;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool AlwaysCanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    } 
}
