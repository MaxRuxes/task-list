using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using AutoMapper;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.ViewModels.Helpers;

namespace TaskList.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : BaseProjectViewModel
    {
        private int _idPriorityType;
        private bool _isEditExistRecord;
        private bool _isEditNow;


        [ImportingConstructor]
        public MainWindowViewModel(IWindowManager windowManager, string connectionString)
        {
            WindowManager = windowManager;
            ConnectionString = connectionString;

            Mapper = new MapperConfiguration((cfg)=> { }).CreateMapper();

            Uow = new DAL.Repositories.EfUnitOfWork(connectionString);

            TodoService = new TodoService(Uow);
            UserService = new UserService(Uow);
            ProjectService = new ProjectService(Uow);

            CurrentPriorityType = "Не выбран";
            NotifyOfPropertyChange(() => CountAllTodo);
        }


        #region Properties

        public string CurrentPriorityType { get; set; }

        public int IdPriorityType
        {
            get => _idPriorityType;
            set
            {
                _idPriorityType = value;
                NotifyOfPropertyChange(() => IdPriorityType);
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

        public ICommand SelectedItemChangedCommand { get; set; }

        #endregion

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

        #endregion

        #region Manage buttons crud

        public void AddTodo()
        {
            IsEditNow = true;
            SelectedItem = new TodoDTO() {StartDate = DateTime.UtcNow, IdPriority = IdPriorityType};
        }

        public void EditTodo()
        {
            IsEditNow = true;
            _isEditExistRecord = true;

            NotifyOfPropertyChange(() => SelectedItem);
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
        }

        public void SaveTodo()
        {
            if (_isEditExistRecord)
            {
                TodoService.UpdateTodo(SelectedItem);
                _isEditExistRecord = false;
            }
            else
            {
                TodoService.CreateTodo(1, CurrentProject.ProjectInfoId, Mapper.Map<TodoDTO, TodoDTO>(SelectedItem));
            }

            UpdateItemCollection(IdPriorityType);
            NotifyOfPropertyChange(() => CountAllTodo);

            IsEditNow = false;
            EditTodoModel = SelectedItem;
        }

        public void CancelTodo()
        {
            _isEditExistRecord = false;
            IsEditNow = false;
            EditTodoModel = SelectedItem;
        }

        #endregion

        #region Helpers methods

        private void UpdateItemCollection(int id)
        {
            TodoItems.Clear();
            TodoService.GetAllTodosForProject(id, CurrentProject.ProjectInfoId)
                .ToList()
                .ForEach(o => TodoItems.Add(o));

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

        public override void Dispose()
        {
            Uow.Dispose();
        }
    }
}
