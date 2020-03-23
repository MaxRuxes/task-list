using System;
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

        private string _login;

        [ImportingConstructor]
        public AuthorizationWindowViewModel(IWindowManager windowManager) :this()
        {
            _windowManager = windowManager;
        }

        public AuthorizationWindowViewModel()
        {
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

        public void SignIn(object xx)
        {

            try {
                var box = (PasswordBox)xx;

                var connectionString = $"Server=127.0.0.1;database=mydb;uid={Login};pwd={box.Password};SslMode=Required;Allow Zero Datetime=true";
                var connection = new MySqlConnection(connectionString);

                try
                {
                    connection.Open();
                    connection.Close();

                    dynamic settings = new ExpandoObject();
                    settings.WinowStartUpLocation = WindowStartupLocation.CenterScreen;

                    _windowManager.ShowWindow(new MainWindowViewModel(_windowManager, connectionString), null, settings);

                    (GetView() as Window).Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
    }
}
