using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;
using TaskList.ViewModels.Helpers;

namespace TaskList.ViewModels
{
    [Export(typeof(StatisticViewModel))]
    public class StatisticViewModel : Screen
    {
        private readonly IUnitOfWork _database;
        private ITodoAndUsersService _todoAndUsersService;
        private IProjectsService _projectsService;
        private IProjectService _projectService;

        public StatisticViewModel(IUnitOfWork database)
        {
            _database = database;

            _projectService = new ProjectService(database);
            _projectsService = new ProjectsService(database);
            _todoAndUsersService = new TodoAndUsersService(database);
        }

        public ObservableCollection<ChartViewModel> Data { get; set; } = new ObservableCollection<ChartViewModel>();

        public string CurrentNameForAxis { get; set; }
        public string CurrentValueForAxis { get; set; }

        public string CurrentNameChart { get; set; }

        public string CurrentDescription { get; set; }

        public int CurrentChart { get; set; }


        public void ShowCostChart()
        {
            Data.Clear();
            CurrentValueForAxis = "Цена/расходы ($)";
            CurrentNameForAxis = "Проекты";

            CurrentNameChart = "Ценовой график";

            CurrentDescription = "";

            var projects = _projectService.GetAllProjects();

            foreach (var projectInfoDto in projects)
            {
                var model = new ChartViewModel();
                model.Name = projectInfoDto.NameProject;
                model.Value = _projectService.GetCostForProject(projectInfoDto.ProjectInfoId);
                Data.Add(model);
            }

            Refresh();
        }

        public void ShowEmployeeChart()
        {
            Data.Clear();

            CurrentValueForAxis = "Сотрудники";
            CurrentNameForAxis = "Проекты";

            CurrentNameChart = "График сотрудников";
            CurrentDescription = "";
            var projects = _projectService.GetAllProjects();

            foreach (var projectInfoDto in projects)
            {
                var model = new ChartViewModel();
                model.Name = projectInfoDto.NameProject;
                model.Value = _projectService.GetCountWorkersForProject(projectInfoDto.ProjectInfoId);
                Data.Add(model);
            }

            Refresh();
        }

        public void ShowTodoChart()
        {
            Data.Clear();

            CurrentValueForAxis = "Человечочасы";
            CurrentNameForAxis = "Проекты";
            CurrentDescription = "";
            CurrentNameChart = "График нагруженности";
            var projects = _projectService.GetAllProjects();

            foreach (var projectInfoDto in projects)
            {
                var model = new ChartViewModel();
                model.Name = projectInfoDto.NameProject;
                model.Value = _projectService.GetCountTodoForProject(projectInfoDto.ProjectInfoId);
                Data.Add(model);
            }

            Refresh();
        }

        public void ShowHoursChart()
        {
            Data.Clear();

            CurrentValueForAxis = "Часы";
            CurrentNameForAxis = "Проекты";
            CurrentDescription = "";
            CurrentNameChart = "Часовой график";

            var projects = _projectService.GetAllProjects();

            foreach (var projectInfoDto in projects)
            {
                var model = new ChartViewModel();
                model.Name = projectInfoDto.NameProject;
                model.Value = _projectService.GetSpentTimeTodoForProject(projectInfoDto.ProjectInfoId);
                Data.Add(model);
            }

            Refresh();
        }

    }
}
