using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Repositories;
using TaskList.Models;
using TaskList.ViewModels.Helpers;

namespace TaskList.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : Screen, IDisposable
    {
        private readonly IWindowManager _windowManager;
        private readonly string _connectionString;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        private readonly ITodoService _todoService;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        private TodoModel _selectedItem;

        private readonly string _signInTime;
        private string _login;

        public int IdPriorityType
        {
            get => _idPriorityType;
            set
            {
                _idPriorityType = value;
                NotifyOfPropertyChange(() => this.IdPriorityType);
            }
        }

        private Visibility _isEditModeVisibility;

        private UserModel _currentUser;


        [ImportingConstructor]
        public MainWindowViewModel(IWindowManager windowManager, string connectionString)
        {
            _windowManager = windowManager;
            _connectionString = connectionString;
            IsEditModeVisibility = Visibility.Hidden;

            _mapper = MapperHelpers.CreateAutoMapper();

            _uow = new DAL.Repositories.EfUnitOfWork(connectionString);

            _todoService = new TodoService(_uow);
            _userService = new UserService(_uow);
            _projectService = new ProjectService(_uow);

            Login = connectionString
                .Split(';')
                .FirstOrDefault(n => n.IndexOf("uid=", StringComparison.Ordinal) != -1)?
                .Substring(4);
            _currentUser = ResolveCurrentUser(Login);

            _signInTime = DateTime.Now.ToUniversalTime().ToLongDateString() + DateTime.Now.ToShortTimeString();
            NotifyOfPropertyChange(() => DateTimeSignIn);
            CurrentPriorityType = "Не выбран";
            NotifyOfPropertyChange(() => CountAllTodos);

            HideShit = Visibility.Visible;
            NotifyOfPropertyChange(() => HideShit);
        }

        private UserModel ResolveCurrentUser(string name)
        {
            var id = default(int);
            switch (name)
            {
                case "root":
                {
                    id = 1;
                    CanEditVisibility = Visibility.Visible;
                    CanEdit = true;
                    break;
                }
                case "manager":
                {
                    id = 2;
                    CanEditVisibility = Visibility.Visible;
                    CanEdit = false;
                    break;
                }
                case "employee":
                {
                    id = 3;
                    CanEditVisibility = Visibility.Collapsed;
                    CanEdit = false;
                    break;
                }
            }

            NotifyOfPropertyChange(() => CanEdit);
            NotifyOfPropertyChange(() => CanEditVisibility);

            return _mapper.Map<UserDTO, UserModel>(_userService.GetUser(id));
        }

        #region Clicks Left panel

        public void ImportAndUrgentCommand()
        {
            IdPriorityType = 1;
            UpdateItemCollection(IdPriorityType);
        }

        public void ImportAndNonUrgentCommand()
        {
            IdPriorityType = 2;
            UpdateItemCollection(IdPriorityType);
        }

        public void UnImportAndUrgentCommand()
        {
            IdPriorityType = 3;
            UpdateItemCollection(IdPriorityType);
        }

        public void UnImportAndNonUrgentCommand()
        {
            IdPriorityType = 4;
            UpdateItemCollection(IdPriorityType);
        }

        public void BackToProjectsCommand()
        {
            _windowManager.ShowWindow(new ProjectsViewModel(_windowManager, _connectionString));
            (GetView() as Window).Close();
        }

        public void OpenUserInfoWindow()
        {
            dynamic settings = new ExpandoObject();
            settings.WinowStartUpLocation = WindowStartupLocation.CenterScreen;

            var teams = _mapper.Map<IEnumerable<ProjectInfoDTO>, List<ProjectModel>>(
                _projectService.GetProjectsForUser(_currentUser.UserId));

            _windowManager.ShowWindow(new UserInfoWindowViewModel(_windowManager, _currentUser, teams), null, settings);
        }

        private void UpdateItemCollection(int id)
        {
            CarouselItems.Clear();
            _todoService.GetAllTodosForProject(id, CurrentProject.ProjectInfoId)
                .ToList()
                .ForEach(o => CarouselItems.Add(_mapper.Map<TodoDTO, TodoModel>(o)));

            NotifyOfPropertyChange(() => CarouselItems);

            SelectedItem = CarouselItems?.FirstOrDefault();

            CurrentPriorityType = ResolveTypeName(id);
            NotifyOfPropertyChange(() => CurrentPriorityType);
        }

        private string ResolveTypeName(int idType)
        {
            switch (idType)
            {
                case 1:
                {
                    return "Важные и срочные";
                }
                case 2:
                {
                    return "Важные и несрочные";
                }
                case 3:
                {
                    return "Неважные и срочные";
                }
                case 4:
                {
                    return "Неважные и несрочные";
                }
                default:
                {
                    return "Важные и срочные";
                }

            }
        }

        #endregion

        #region Manage buttons crud

        private bool _isEditExistRecord;
        private Visibility _hideShit;
        private int _idPriorityType;

        public void AddTodo()
        {
            SelectedItem = new TodoModel() {StartDate = DateTime.UtcNow, IdPriority = IdPriorityType};

            IsEditModeVisibility = Visibility.Visible;
            NotifyOfPropertyChange(() => IsEditModeVisibility);

            HideShit = Visibility.Collapsed;
            NotifyOfPropertyChange(() => HideShit);
        }

        public void EditTodo()
        {
            _isEditExistRecord = true;

            NotifyOfPropertyChange(() => SelectedItem);

            IsEditModeVisibility = Visibility.Visible;
            NotifyOfPropertyChange(() => IsEditModeVisibility);

            HideShit = Visibility.Collapsed;
            NotifyOfPropertyChange(() => HideShit);
        }

        public void DeleteTodo()
        {
            if (SelectedItem == null)
            {
                return;
            }

            _todoService.DeleteTodo(SelectedItem.TodoId);
            System.Windows.Forms.MessageBox.Show("Успешно удалено!");
            UpdateItemCollection(IdPriorityType);

            IsEditModeVisibility = Visibility.Collapsed;
            NotifyOfPropertyChange(() => IsEditModeVisibility);
        }

        public void SaveTodo()
        {
            IsEditModeVisibility = Visibility.Collapsed;
            HideShit = Visibility.Visible;

            NotifyOfPropertyChange(() => IsEditModeVisibility);
            NotifyOfPropertyChange(() => HideShit);

            if (_isEditExistRecord)
            {
                _todoService.UpdateTodo(_mapper.Map<TodoModel, TodoDTO>(SelectedItem));
                _isEditExistRecord = false;
            }
            else
            {
                _todoService.CreateTodo(_currentUser.UserId, CurrentProject.ProjectInfoId, _mapper.Map<TodoModel, TodoDTO>(SelectedItem));
            }

            UpdateItemCollection(IdPriorityType);
            NotifyOfPropertyChange(() => CountAllTodos);

        }

        public void CancelTodo()
        {
            _isEditExistRecord = false;
            IsEditModeVisibility = Visibility.Collapsed;
            HideShit = Visibility.Visible;

            NotifyOfPropertyChange(() => IsEditModeVisibility);
            NotifyOfPropertyChange(() => HideShit);
        }

        #endregion

        public Visibility CanEditVisibility { get; set; }

        public Visibility IsEditModeVisibility
        {
            get => _isEditModeVisibility;
            set
            {
                _isEditModeVisibility = value;
                NotifyOfPropertyChange(() => _isEditModeVisibility);
            }
        }

        public Visibility HideShit
        {
            get => _hideShit;
            set
            {
                _hideShit = value;
                NotifyOfPropertyChange(() => HideShit);
            }
        }


        public bool CanEdit { get; set; }

        public TodoModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public string CountAllTodos => GetAllTodosForProjectCount().ToString();

        public string CurrentPriorityType { get; set; }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                NotifyOfPropertyChange(() => Login);
            }
        }

        public string DateTimeSignIn => _signInTime;

        public ObservableCollection<TodoModel> CarouselItems { get; set; } = new ObservableCollection<TodoModel>();
        public ProjectInfoDTO CurrentProject { get; internal set; }

        private int GetAllTodosForProjectCount()
        {
            return ((TodoAndProjectsRepository) _uow.TodoAndProjects).GetCountForProject(CurrentProject.ProjectInfoId);
        }

        public void Dispose()
        {
            _uow.Dispose();
        }
    }
}
