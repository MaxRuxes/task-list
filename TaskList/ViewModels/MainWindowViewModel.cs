using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace TaskList.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : PropertyChangedBase
    {
        private readonly IWindowManager _windowManager;

        private string _signInTime;
        private string _login;

        [ImportingConstructor]
        public MainWindowViewModel(IWindowManager windowManager, string connectionString)
        {
            _windowManager = windowManager;
            Login = connectionString.Split(';').ToList().Where(n => n.IndexOf("uid=") != -1).ToList()[0];
            _signInTime = DateTime.Now.ToUniversalTime().ToLongDateString() + DateTime.Now.ToShortTimeString();

            NotifyOfPropertyChange(() => DateTimeSignIn);

            for (int i = 0; i < 20; i++)
            {
                CarouselItems.Add($"item{i.ToString()}");
            }

            using (DAL.TaskListContext context = new DAL.TaskListContext(connectionString))
            {
                CarouselItems.Clear();
                context.Attachments.ToList().ForEach(n=> CarouselItems.Add(n.Content));
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
