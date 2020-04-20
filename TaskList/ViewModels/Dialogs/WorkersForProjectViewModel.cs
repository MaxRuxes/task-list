using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using Syncfusion.Data.Extensions;
using Syncfusion.Windows.Shared;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;

namespace TaskList.ViewModels
{
    [Export(typeof(WorkersForProjectViewModel))]
    public class WorkersForProjectViewModel : Screen
    {
        private UserDTO _selectedWorker;
        private UserDTO _editWorker;
        private bool _isEditNow;
        private readonly IUserService _userService;

        public WorkersForProjectViewModel(IUnitOfWork unitOfWork)
        {
            var userService = new UserService(unitOfWork);
            Workers = new ObservableCollection<UserDTO>(userService.GetAllUsers());
            _userService = new UserService(unitOfWork);
        }

        public ObservableCollection<UserDTO> Workers { get; set; }

        public UserDTO SelectedWorker
        {
            get => _selectedWorker;
            set
            {
                _selectedWorker = value;
                EditWorker = value != null
                    ? value.Copy()
                    : new UserDTO();

                NotifyOfPropertyChange(() => SelectedWorker);
                NotifyOfPropertyChange(() => IsSelectedUserExist);
            }
        }

        public bool IsSelectedUserExist => SelectedWorker != null;

        public UserDTO EditWorker
        {
            get => _editWorker;
            set
            {
                _editWorker = value;
                NotifyOfPropertyChange(() => EditWorker);
            }
        }

        public bool IsEditNow
        {
            get => _isEditNow;
            set
            {
                _isEditNow = value;
                NotifyOfPropertyChange(() => IsEditNow);
            }
        }

        public void SaveUser()
        {
            if (EditWorker.FullName == null || EditWorker.FullName.IsNullOrWhiteSpace() ||
                EditWorker.Role == null || EditWorker.Role.IsNullOrWhiteSpace() ||
                EditWorker.RatePerHour <= 0)
            {
                MessageBox.Show("Проверьте правильность введенных данных.",
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                return;
            }

            IsEditNow = false;
            if (EditWorker.UserId == -1)
            {
                EditWorker.UserId = 0;
                _userService.CreateUser(EditWorker);
            }
            else
            {
                _userService.UpdateUser(EditWorker);
            }

            UpdateData();
        }

        public void CancelUser()
        {
            IsEditNow = false;
            EditWorker = SelectedWorker.Copy();
        }

        public void CreateUserCommand()
        {
            EditWorker = new UserDTO {UserId = -1};
            IsEditNow = true;
        }

        public void RemoveUserCommand()
        {
            IsEditNow = false;
            _userService.ChangeActiveForUser(SelectedWorker.UserId, !SelectedWorker.IsActive);
            UpdateData();
        }

        public void RenameUserCommand()
        {
            IsEditNow = true;
        }

        private void UpdateData()
        {
            Workers.Clear();
            var list = _userService.GetAllUsers();
            list.ForEach(x => Workers.Add(x));
        }
    }
}
