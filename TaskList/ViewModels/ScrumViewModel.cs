using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Caliburn.Micro;
using Syncfusion.UI.Xaml.Kanban;
using Syncfusion.Windows.Shared;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.ToolKit.ViewModel;
using TaskList.ViewModels.Helpers;
using TaskList.ViewModels.Models;

namespace TaskList.ViewModels
{
    public class ScrumViewModel : BaseProjectViewModel
    {
        private bool _isEditExistRecord;

        public ScrumViewModel(IWindowManager windowManager, string connectionString, ProjectInfoDTO project)
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

            SelectedCommand = new RelayCommand((SelectItemChangedCommandExecute));

            StartExecuteCommand = new RelayCommand(StartExecuteCommandExecute);
            EndExecuteCommand = new RelayCommand(EndExecuteCommandExecute);

            UpdateData();
        }

        public ICommand SelectedCommand { get; set; }

        public ObservableCollection<CustomKanbanModel> KanbanModels { get; set; }

        public CustomKanbanModel CurrentTodo { get; set; }

        public void AddTodo()
        {
            IsEditNow = true;
            EditTodoModel = new TodoModel {State = -1, IdPriority = 1};
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
            UpdateData();
            SelectedItem = null;
            System.Windows.Forms.MessageBox.Show(@"Успешно удалено!");
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

            NotifyOfPropertyChange(() => CountAllTodo);

            UpdateData();

            IsEditNow = false;
            EditTodoModel = SelectedItem;
        }

        public void CancelTodo()
        {
            _isEditExistRecord = false;
            IsEditNow = false;
            UpdateData();
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

        private static string ResolveStateToColorKey(int state)
        {
            switch (state)
            {
                case -1:
                {
                    return "Low";
                }
                case 0:
                {
                    return "Normal";
                }
                case 1:
                {
                    return "Low";
                }
                default:
                {
                    return "High";
                }
            }
        }

        private static string ResolveStateToCategory(int state)
        {
            switch (state)
            {
                case -1:
                {
                    return "Open";
                }
                case 0:
                {
                    return "In Progress";
                }
                case 1:
                {
                    return "Done";
                }
                default:
                {
                    return "Review";
                }
            }
        }

        private void UpdateData()
        {
            var list = TodoService.GetAllTodo(CurrentProject.ProjectInfoId);
            KanbanModels = new ObservableCollection<CustomKanbanModel>(list
                .Select(x =>
                {
                    var model = new CustomKanbanModel(x)
                    {
                        CurrentUser = TodoAndUsersService.GetUserForTodo(x.TodoId)
                    };

                    model.Assignee = model.CurrentUser.FullName;
                    model.Title = x.Caption;
                    model.ID = x.TodoId.ToString();
                    model.Description = x.Content;
                    model.Category = ResolveStateToCategory(x.State);
                    model.ColorKey = ResolveStateToColorKey(x.State);
                    model.ImageURL = new Uri(@"D:\Projects\TaskList\TaskList\ToolKit\iconsMan.png",
                        UriKind.RelativeOrAbsolute);
                    return model;
                }));

            Refresh();
        }

        private void SelectItemChangedCommandExecute(object paramArgs)
        {
            var args = (KanbanDragEventArgs) paramArgs;
            var item = Mapper.Map<TodoDTO, TodoModel>(((CustomKanbanModel) args.SelectedCard.Content).CurrentTodo);
            item.Owner = ((CustomKanbanModel) args.SelectedCard.Content).CurrentUser;
            SelectedItem = item;
            Refresh();
        }

        public override void Dispose()
        {
        }
    }
}
