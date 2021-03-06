﻿using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.ToolKit.ViewModel;
using TaskList.ViewModels.Models;
using Syncfusion.Windows.Shared;

namespace TaskList.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : BaseProjectViewModel
    {
        private int _idPriorityType;
        private bool _isEditExistRecord;


        [ImportingConstructor]
        public MainWindowViewModel(IWindowManager windowManager, string connectionString, ProjectInfoDTO project)
            : base(project)
        {
            WindowManager = windowManager;
            ConnectionString = connectionString;

            Mapper = new MapperConfiguration((cfg) =>
            {
                cfg.CreateMap<TodoDTO, TodoModel>()
                    .ForMember(x => x.Owner, q => q.Ignore())
                    .ForMember(x => x.StateString, q => q.Ignore());
                cfg.CreateMap<TodoModel, TodoDTO>();

                cfg.CreateMap<PriorityTypeDTO, PriorityModel>()
                    .ForMember(x => x.PriorityContent, x => x.MapFrom(m => m.NamePriority))
                    .ForMember(x => x.PriorityId, x => x.MapFrom(m => m.PriorityTypeId));
                cfg.CreateMap<PriorityModel, PriorityTypeDTO>()
                    .ForMember(x => x.NamePriority, x => x.MapFrom(m => m.PriorityContent))
                    .ForMember(x => x.PriorityTypeId, x => x.MapFrom(m => m.PriorityId));

            }).CreateMapper();

            Uow = new DAL.Repositories.EfUnitOfWork(connectionString);

            TodoService = new TodoService(Uow);
            UserService = new UserService(Uow);
            ProjectService = new ProjectService(Uow);
            TodoAndUsersService = new TodoAndUsersService(Uow);

            CurrentPriorityType = "Не выбран";
            NotifyOfPropertyChange(() => CountAllTodo);

            StartExecuteCommand = new RelayCommand(StartExecuteCommandExecute);
            EndExecuteCommand = new RelayCommand(EndExecuteCommandExecute);
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
            EditTodoModel = new TodoModel {State = -1, IdPriority = IdPriorityType};
        }

        public void EditTodo()
        {
            IsEditNow = true;
            _isEditExistRecord = true;
        }

        public void DeleteTodo()
        {
            if (EditTodoModel == null)
            {
                return;
            }

            TodoService.DeleteTodo(EditTodoModel.TodoId, CurrentProject.ProjectInfoId);
            System.Windows.Forms.MessageBox.Show(@"Успешно удалено!");
            UpdateItemCollection(IdPriorityType);
        }

        public void SaveTodo()
        {
            if (EditTodoModel.Caption == null || EditTodoModel.Caption.IsNullOrWhiteSpace() ||
                EditTodoModel.Content == null || EditTodoModel.Content.IsNullOrWhiteSpace() ||
                EditTodoModel.EstimatedHours <= 0)
            {
                MessageBox.Show(
                    $"Проверьте правильность введенных данных.",
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                return;
            }

            if (EditTodoModel.Owner == null)
            {
                MessageBox.Show(
                    $"Необходимо назначить исполнителя для текущей задачи.",
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            if (_isEditExistRecord)
            {
                TodoService.UpdateTodo(Mapper.Map<TodoModel, TodoDTO>(EditTodoModel), EditTodoModel.Owner);
                _isEditExistRecord = false;
            }
            else
            {
                TodoService.CreateTodo(CurrentProject.ProjectInfoId, Mapper.Map<TodoModel, TodoDTO>(EditTodoModel),
                    EditTodoModel.Owner);
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

        public void ChangeCurrentOwner()
        {
            var viewModel = new WorkersSelectorViewModel(Uow, new[] {EditTodoModel.Owner});
            if (WindowManager.ShowDialog(viewModel) != true)
            {
                return;
            }

            EditTodoModel.Owner = viewModel.SelectedWorker;
            Refresh();
        }

        #endregion

        #region Helpers methods

        private void UpdateItemCollection(int id)
        {
            TodoItems.Clear();

            TodoService.GetAllTodosForProject(id, CurrentProject.ProjectInfoId)
                .ToList()
                .ForEach(o =>
                {
                    var owner = TodoAndUsersService.GetUserForTodo(o.TodoId);
                    var mappingModel = Mapper.Map<TodoDTO, TodoModel>(o);
                    mappingModel.Owner = owner;
                    mappingModel.StateString = ResolveState(mappingModel.State);
                    TodoItems.Add(mappingModel);
                });

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

        private string ResolveState(int state)
        {
            switch (state)
            {
                case -1:
                    return "Открыт";
                case 0:
                    return "В процессе";
                default:
                    return "Выполнен";

            }

        }

        #endregion


        public override void Dispose()
        {
            Uow.Dispose();
        }
    }
}
