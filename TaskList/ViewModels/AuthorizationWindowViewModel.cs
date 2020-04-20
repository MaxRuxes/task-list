using System;
using Caliburn.Micro;
using System.Windows;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

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
            Task.Run(() => Start());
        }

        private async void Start()
        {
            await Task.Delay(1500);

            try
            {
                var connectionString =
                    $"Server=127.0.0.1;database=mydb;uid=root;pwd=1234;SslMode=Required;Allow Zero Datetime=true";
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось подключиться к базе данных. Убедитесь в доступности и наличии базы данных. Подробности исключения:\n\n{ex.Message}",
                    "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Environment.Exit(1);
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                _windowManager.ShowWindow(new ProjectsViewModel(_windowManager));
                (GetView() as Window)?.Close();
            }, DispatcherPriority.ContextIdle);
        }
    }
}
