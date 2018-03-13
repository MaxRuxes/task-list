using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;

namespace TaskList.ViewModels
{
    public class AuthorizationWindowViewModel : PropertyChangedBase
    {
        private bool _enabledButton;
        private string _password;
        private string _login;

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

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
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
                MessageBox.Show("AllGood");
                await connection.CloseAsync();
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
