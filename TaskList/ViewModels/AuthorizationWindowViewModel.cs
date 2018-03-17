using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.Dynamic;

namespace TaskList.ViewModels
{
    [Export(typeof(AuthorizationWindowViewModel))]
    public class AuthorizationWindowViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        private bool _enabledButton;
        private string _login;

        [ImportingConstructor]
        public AuthorizationWindowViewModel(IWindowManager windowManager) :this()
        {
            _windowManager = windowManager;
        }

        public AuthorizationWindowViewModel()
        {
            EnabledButton = true;
        }

        public bool EnabledButton
        {
            get => _enabledButton;
            set
            {
                _enabledButton = value;
                NotifyOfPropertyChange(() => EnabledButton);
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                NotifyOfPropertyChange(() => Login);
            }
        }

        public void CancelCommand()
        {
            Environment.Exit(0);
        }

        public async Task SignIn(object xx)
        {
            EnabledButton = false;

            var box = (PasswordBox)xx;

            var connectionString = $"Server=localhost;database=mydb;uid={Login};pwd={box.Password};pooling=true;";
            var connection = new MySqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();
                await connection.CloseAsync();

                dynamic settings = new ExpandoObject(); 
                settings.WinowStartUpLocation = WindowStartupLocation.CenterScreen;

                _windowManager.ShowWindow(new MainWindowViewModel(_windowManager, Login), null, settings);

                (GetView() as Window).Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                EnabledButton = true;
                connection.Dispose();
            }
        }
    }
}
