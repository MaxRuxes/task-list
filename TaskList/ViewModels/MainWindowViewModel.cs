using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace TaskList.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : PropertyChangedBase
    {
        private readonly IWindowManager _windowManager;

        private string _signInTime;
        private string _login;

        [ImportingConstructor]
        public MainWindowViewModel(IWindowManager windowManager, string login)
        {
            _windowManager = windowManager;
            Login = login;
            _signInTime = DateTime.Now.ToUniversalTime().ToLongDateString() + DateTime.Now.ToShortTimeString();

            NotifyOfPropertyChange(() => DateTimeSignIn);

            for (int i = 0; i < 20; i++)
            {
                CarouselItems.Add($"item{i.ToString()}");
            }
        }

        public string Login
        {
            get =>_login;
            set
            {
                _login = value;
                NotifyOfPropertyChange(() => Login);
            }
        }

        public string DateTimeSignIn => _signInTime;

        public ObservableCollection<string> CarouselItems { get; set; } = new ObservableCollection<string>();
    }
}
