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
        private readonly IProjectService _projectService;

        public StatisticViewModel(IUnitOfWork database)
        {
            _projectService = new ProjectService(database);
        }

        public ObservableCollection<ChartViewModel> ChartData { get; set; } =
            new ObservableCollection<ChartViewModel>();

        public string CurrentNameForAxis { get; set; }
        public string CurrentValueForAxis { get; set; }
        public string CurrentNameChart { get; set; }

        public void ShowCostChart()
        {
            ChartData.Clear();

            CurrentValueForAxis = "Цена/расходы ($)";
            CurrentNameForAxis = "Проекты";
            CurrentNameChart = "Ценовой график";

            var projects = _projectService.GetAllProjects();
            foreach (var projectInfoDto in projects)
            {
                var model = new ChartViewModel
                {
                    Name = projectInfoDto.NameProject,
                    Value = _projectService.GetCostForProject(projectInfoDto.ProjectInfoId)
                };

                ChartData.Add(model);
            }

            Refresh();
        }

        public void ShowEmployeeChart()
        {
            ChartData.Clear();

            CurrentValueForAxis = "Сотрудники";
            CurrentNameForAxis = "Проекты";
            CurrentNameChart = "График сотрудников";

            var projects = _projectService.GetAllProjects();
            foreach (var projectInfoDto in projects)
            {
                var model = new ChartViewModel
                {
                    Name = projectInfoDto.NameProject,
                    Value = _projectService.GetCountWorkersForProject(projectInfoDto.ProjectInfoId)
                };

                ChartData.Add(model);
            }

            Refresh();
        }

        public void ShowTodoChart()
        {
            ChartData.Clear();

            CurrentValueForAxis = "Человечочасы";
            CurrentNameForAxis = "Проекты";
            CurrentNameChart = "График нагруженности";

            var projects = _projectService.GetAllProjects();
            foreach (var projectInfoDto in projects)
            {
                var model = new ChartViewModel
                {
                    Name = projectInfoDto.NameProject,
                    Value = _projectService.GetCountTodoForProject(projectInfoDto.ProjectInfoId)
                };

                ChartData.Add(model);
            }

            Refresh();
        }

        public void ShowHoursChart()
        {
            ChartData.Clear();

            CurrentValueForAxis = "Часы";
            CurrentNameForAxis = "Проекты";
            CurrentNameChart = "Часовой график";

            var projects = _projectService.GetAllProjects();
            foreach (var projectInfoDto in projects)
            {
                var model = new ChartViewModel
                {
                    Name = projectInfoDto.NameProject,
                    Value = _projectService.GetSpentTimeTodoForProject(projectInfoDto.ProjectInfoId)
                };

                ChartData.Add(model);
            }

            Refresh();
        }
    }
}
