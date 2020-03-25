using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Windows;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;
using TaskList.Models;

namespace TaskList.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : PropertyChangedBase, IDisposable
    {
        private readonly IWindowManager _windowManager;
        private IUnitOfWork uow;
        private IMapper mapper;

        private ITodoService todoService;
        private IUserService userService;
        private IProjectService teamService;

        private TodoModel _selectedItem;

        private string _signInTime;
        private string _login;
        private int idPriorityType;
        private Visibility _isEditModeVisibility;

        private UserModel CurrentUser { get; set; }


        [ImportingConstructor]
        public MainWindowViewModel(IWindowManager windowManager, string connectionString)
        {
            _windowManager = windowManager;

            IsEditModeVisibility = Visibility.Hidden;

            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TodoDTO, TodoModel>()
                    .ForMember(x => x.ContentTodo, x => x.MapFrom(m => m.Content));
                cfg.CreateMap<TodoModel, TodoDTO>()
                    .ForMember(x => x.Content, x => x.MapFrom(m => m.ContentTodo));

                cfg.CreateMap<PriorityTypeDTO, PriorityModel>()
                    .ForMember(x => x.PriorityContent, x => x.MapFrom(m => m.NamePriority))
                    .ForMember(x => x.PriorityId, x => x.MapFrom(m => m.PriorityTypeId));
                cfg.CreateMap<PriorityModel, PriorityTypeDTO>()
                    .ForMember(x => x.NamePriority, x => x.MapFrom(m => m.PriorityContent))
                    .ForMember(x => x.PriorityTypeId, x => x.MapFrom(m => m.PriorityId));

                cfg.CreateMap<UserDTO, UserModel>().ForMember(x => x.Role, x => x.MapFrom(m => m.Role));
                cfg.CreateMap<UserModel, UserDTO>();
            }).CreateMapper();

            uow = new DAL.Repositories.EFUnitOfWork(connectionString);

            todoService = new TodoService(uow);
            userService = new UserService(uow);
            teamService = new ProjectService(uow);

            Login = connectionString.Split(';').ToList().Where(n => n.IndexOf("uid=") != -1).ToList()[0].Substring(4);
            CurrentUser = ResolveCurrentUser(Login);

            _signInTime = DateTime.Now.ToUniversalTime().ToLongDateString() + DateTime.Now.ToShortTimeString();
            NotifyOfPropertyChange(() => DateTimeSignIn);

            ImportAndUrgentCommand();
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

            return mapper.Map<UserDTO, UserModel>(userService.GetUser(id));
        }

        #region Clicks Left panel
        public void ImportAndUrgentCommand()
        {
            idPriorityType = 1;
            UpdateItemCollection(idPriorityType);
        }

        public void ImportAndNonUrgentCommand()
        {
            idPriorityType = 2;
            UpdateItemCollection(idPriorityType);
        }

        public void UnImportAndUrgentCommand()
        {
            idPriorityType = 3;
            UpdateItemCollection(idPriorityType);
        }

        public void UnImportAndNonUrgentCommand()
        {
            idPriorityType = 4;
            UpdateItemCollection(idPriorityType);
        }

        public void OpenUserInfoWindow()
        {
            dynamic settings = new ExpandoObject();
            settings.WinowStartUpLocation = WindowStartupLocation.CenterScreen;

            var teams = mapper.Map<IEnumerable<ProjectInfoDTO>, List<TeamModel>>(teamService.GetTeamsForUser(CurrentUser.UserId));

            _windowManager.ShowWindow(new UserInfoWindowViewModel(_windowManager, CurrentUser, teams), null, settings);
        }

        private void UpdateItemCollection(int id)
        {
            CarouselItems.Clear();
            todoService.GetAllTodos(id).ToList().ForEach(o => CarouselItems.Add(mapper.Map<TodoDTO, TodoModel>(o)));
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

        private bool isEditExistRecord;
        private Visibility _hideShit;

        public void AddTodo()
        {
            SelectedItem = new TodoModel() { StartDate = DateTime.UtcNow, IdPriority = idPriorityType };

            IsEditModeVisibility = Visibility.Visible;
            NotifyOfPropertyChange(() => IsEditModeVisibility);

            HideShit = Visibility.Collapsed;
            NotifyOfPropertyChange(() => HideShit);
        }

        public void EditTodo()
        {
            isEditExistRecord = true;

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

            todoService.DeleteTodo(SelectedItem.TodoId);
            System.Windows.Forms.MessageBox.Show("Успешно удалено!");
            UpdateItemCollection(idPriorityType);

            IsEditModeVisibility = Visibility.Collapsed;
            NotifyOfPropertyChange(() => IsEditModeVisibility);
        }

        public void SaveTodo()
        {
            IsEditModeVisibility = Visibility.Collapsed;
            HideShit = Visibility.Visible;

            NotifyOfPropertyChange(() => IsEditModeVisibility);
            NotifyOfPropertyChange(() => HideShit);

            if (isEditExistRecord)
            {
                todoService.UpdateTodo(mapper.Map<TodoModel, TodoDTO>(SelectedItem));
                isEditExistRecord = false;
            }
            else
            {
                todoService.CreateTodo(CurrentUser.UserId, mapper.Map<TodoModel, TodoDTO>(SelectedItem));
            }

            UpdateItemCollection(idPriorityType);
            NotifyOfPropertyChange(() => CountAllTodos);

        }

        public void CancelTodo()
        {
            isEditExistRecord = false;
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

        public string CountAllTodos => GetAllTodosCount().ToString();

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


        private int GetAllTodosCount()
        {
            if (Login != "root")
                return 0;
            return ((DAL.Repositories.EFUnitOfWork)uow).Database.SqlQuery<int>("select CountAllTodoForAllUsers();").FirstOrDefault();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
