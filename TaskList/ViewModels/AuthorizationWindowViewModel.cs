using Caliburn.Micro;
using System.Windows;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TaskList.ViewModels
{
    [Export(typeof(AuthorizationWindowViewModel))]
    public class AuthorizationWindowViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        [ImportingConstructor]
        public AuthorizationWindowViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            Task.Run(Start);
        }

        private async void Start()
        {
            await Task.Delay(3000);
            Application.Current.Dispatcher.Invoke(() =>
            {
                _windowManager.ShowWindow(new ProjectsViewModel(_windowManager));
                (GetView() as Window)?.Close();
            }, DispatcherPriority.ContextIdle);
        }
    }
}
