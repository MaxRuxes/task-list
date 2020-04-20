using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;

namespace TaskList.ViewModels
{
    [Export(typeof(ProjectInfoViewModel))]
    public class WorkersSelectorViewModel :Screen
    {
        public WorkersSelectorViewModel(IUnitOfWork unitOfWork, IEnumerable<UserDTO> exists)
        {
            var userService = new UserService(unitOfWork);
            var list = userService.GetAllUsers()
                .Where(x => x.IsActive && exists.Where(q => q != null).All(q => q.UserId != x.UserId))
                .Select(x => x);
            Workers = new ObservableCollection<UserDTO>(list);
        }

        public ObservableCollection<UserDTO> Workers { get; set; }
        public UserDTO SelectedWorker { get; set; }

        public void SelectUsersCommand()
        {
            TryClose(true);
        }
    }
}
