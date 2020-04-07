using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.Models;
using TaskList.ViewModels.Helpers;

namespace TaskList.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : BaseProjectViewModel
    {

        public int IdPriorityType
        {
            get => _idPriorityType;
            set
            {
                _idPriorityType = value;
                NotifyOfPropertyChange(() => IdPriorityType);
            }
        }

        private Visibility _isEditModeVisibility;

        [ImportingConstructor]
        public MainWindowViewModel(IWindowManager windowManager, string connectionString)
        {
            WindowManager = windowManager;
            ConnectionString = connectionString;
            IsEditModeVisibility = Visibility.Hidden;

            Mapper = MapperHelpers.CreateAutoMapper();

            Uow = new DAL.Repositories.EfUnitOfWork(connectionString);

            TodoService = new TodoService(Uow);
            UserService = new UserService(Uow);
            ProjectService = new ProjectService(Uow);

            Login = connectionString
                .Split(';')
                .FirstOrDefault(n => n.IndexOf("uid=", StringComparison.Ordinal) != -1)?
                .Substring(4);
            CurrentUser = ResolveCurrentUser(Login);

            SignInTime = DateTime.Now.ToUniversalTime().ToLongDateString() + DateTime.Now.ToShortTimeString();
            NotifyOfPropertyChange(() => DateTimeSignIn);
            CurrentPriorityType = "Не выбран";
            NotifyOfPropertyChange(() => CountAllTodo);

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

            return Mapper.Map<UserDTO, UserModel>(UserService.GetUser(id));
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
        
        private void UpdateItemCollection(int id)
        {
            TodoItems.Clear();
            TodoService.GetAllTodosForProject(id, CurrentProject.ProjectInfoId)
                .ToList()
                .ForEach(o => TodoItems.Add(Mapper.Map<TodoDTO, TodoModel>(o)));

            NotifyOfPropertyChange(() => TodoItems);

            SelectedItem = TodoItems?.FirstOrDefault();

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

            TodoService.DeleteTodo(SelectedItem.TodoId, CurrentProject.ProjectInfoId);
            System.Windows.Forms.MessageBox.Show(@"Успешно удалено!");
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
                TodoService.UpdateTodo(Mapper.Map<TodoModel, TodoDTO>(SelectedItem));
                _isEditExistRecord = false;
            }
            else
            {
                TodoService.CreateTodo(CurrentUser.UserId, CurrentProject.ProjectInfoId, Mapper.Map<TodoModel, TodoDTO>(SelectedItem));
            }

            UpdateItemCollection(IdPriorityType);
            NotifyOfPropertyChange(() => CountAllTodo);

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

        public string CurrentPriorityType { get; set; }
        
        public override void Dispose()
        {
            Uow.Dispose();
        }
    }
}
