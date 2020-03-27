using Caliburn.Micro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using TaskList.Models;

namespace TaskList.ViewModels
{
    [Export(typeof(UserInfoWindowViewModel))]
    public class UserInfoWindowViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        [ImportingConstructor]
        public UserInfoWindowViewModel(IWindowManager windowManager, UserModel user, List<ProjectModel> teams)
        {
            _windowManager = windowManager;

            Currentuser = user;
            Teams = new ObservableCollection<ProjectModel>(teams);
        }

        public UserModel Currentuser { get; }

        public ObservableCollection<ProjectModel> Teams { get; }

        public void CloseCurrentWindow()
        {
            (GetView() as Window).Close();
        }

    }
}
