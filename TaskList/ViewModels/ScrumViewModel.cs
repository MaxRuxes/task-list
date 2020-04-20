using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using Syncfusion.UI.Xaml.Kanban;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.ViewModels.Helpers;

namespace TaskList.ViewModels
{
    public class ScrumViewModel : BaseProjectViewModel
    {
        public ScrumViewModel(IWindowManager windowManager, string connectionString, ProjectInfoDTO project)
            : base(project)
        {
            WindowManager = windowManager;
            ConnectionString = connectionString;

            Mapper = new MapperConfiguration((cfg) => { }).CreateMapper();

            Uow = new DAL.Repositories.EfUnitOfWork(connectionString);

            TodoService = new TodoService(Uow);
            UserService = new UserService(Uow);
            ProjectService = new ProjectService(Uow);
            var usersAndTodoService = new TodoAndUsersService(Uow);

            var list = TodoService.GetAllTodo(CurrentProject.ProjectInfoId);
            KanbanModels = new ObservableCollection<CustomKanbanModel>(list
                .Select(x =>
                {
                    var model = new CustomKanbanModel(x);
                    model.CurrentUser = usersAndTodoService.GetUserForTodo(x.TodoId);
                    model.Assignee = model.CurrentUser.FullName;
                    model.Title = x.Caption;
                    model.ID = x.TodoId.ToString();
                    model.Description = x.Content;
                    model.Category = ResolveStateToCategory(x.State);
                    model.ColorKey = ResolveStateToColorKey(x.State);
                    return model;
                }));
        }

        public ObservableCollection<CustomKanbanModel> KanbanModels { get; set; }

        public CustomKanbanModel CurrentTodo { get; set; }

        private static string ResolveStateToCategory(int state)
        {
            switch (state)
            {
                case -1:
                    return "Open";
                case 0:
                    return "In Progress";
                case 1:
                    return "Done";
                default:
                    return "Review";
            }
        }

        private static string ResolveStateToColorKey(int state)
        {
            switch (state)
            {
                case -1:
                    return "Low";
                case 0:
                    return "Normal";
                case 1:
                    return "Low";
                default:
                    return "High";
            }
        }

        public override void Dispose()
        {
        }
    }
}
