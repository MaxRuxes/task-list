using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using TaskList.DAL.Models;

namespace TaskList.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : PropertyChangedBase
    {
        private readonly IWindowManager _windowManager;

        private string _signInTime;
        private string _login;
        private string _selectedItem;

        [ImportingConstructor]
        public MainWindowViewModel(IWindowManager windowManager, string connectionString)
        {
            _windowManager = windowManager;
            Login = connectionString.Split(';').ToList().Where(n => n.IndexOf("uid=") != -1).ToList()[0];
            _signInTime = DateTime.Now.ToUniversalTime().ToLongDateString() + DateTime.Now.ToShortTimeString();

            NotifyOfPropertyChange(() => DateTimeSignIn);


            using (var context = new DAL.Repositories.EFUnitOfWork(connectionString))
            {

                //var item = (Attachments)context.Attachments.Find(o => o.Content == "записывайся блять").FirstOrDefault();

                //if (item != null)
                //{
                //    item.Content = "пяздец";
                //    context.Attachments.Update(item);
                //    context.Save();
                //}

                //CarouselItems.Clear();
                context.Attachments.GetAll().ToList().ForEach(n => CarouselItems.Add(n.Content));
                SelectedItem = CarouselItems.First();

                //var item = new DAL.Models.Attachments() { AttachTypeId = 2, Content = "записывайся блять", CreateDate = DateTime.UtcNow.Date};
                //context.Attachments.Create(item);
                

            }
        }

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
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
