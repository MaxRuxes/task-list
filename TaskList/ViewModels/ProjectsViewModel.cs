using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using MySql.Data.MySqlClient;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;

namespace TaskList.ViewModels
{
    [Export(typeof(ProjectsViewModel))]
    public class ProjectsViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly string _connectionString;

        private readonly IProjectService _projectService;
        private readonly IUnitOfWork _uow;
        private readonly IProjectsService _projectsService;
        private ProjectInfoDTO _currentProject;

        public ProjectsViewModel(IWindowManager windowManager)
        {

            var connectionString = $"Server=127.0.0.1;database=mydb;uid=root;pwd=1234;SslMode=Required;Allow Zero Datetime=true";

            _windowManager = windowManager;
            _connectionString = connectionString;
            
            _uow = new DAL.Repositories.EfUnitOfWork(connectionString);
            _projectService = new ProjectService(_uow);
            _projectsService = new ProjectsService(_uow);

            var projectsNames = _projectService.GetAllProjects().ToList();

            Projects = new ObservableCollection<ProjectInfoDTO>(projectsNames);
            CurrentProject = Projects.FirstOrDefault();
        }

        public ObservableCollection<ProjectInfoDTO> Projects { get; set; }

        public bool IsCurrentProjectNull => CurrentProject != null;

        public ProjectInfoDTO CurrentProject
        {
            get => _currentProject;
            set
            {
                _currentProject = value;
                NotifyOfPropertyChange(() => CurrentProject);
                NotifyOfPropertyChange(()=> IsCurrentProjectNull);
            }
        }


        public void SelectProject(ProjectInfoDTO project)
        {
            BaseProjectViewModel viewModel;

            if (CurrentProject.IsAgile)
            {
                viewModel = new MainWindowViewModel(_windowManager, _connectionString, project);
            }
            else
            {
                viewModel = new ScrumViewModel(_windowManager, _connectionString, project);
            }

            _windowManager.ShowDialog(viewModel);

            (GetView() as Window)?.Close();
        }

        public void CreateProjectCommand()
        {
            var vm = new ProjectInfoViewModel(_windowManager, _uow, 0)
            {
                IsAgile = true
            };
            vm.IsScrum = !vm.IsAgile;
            if (_windowManager.ShowDialog(vm) != true)
            {
                return;
            }

            var newProject = _projectService.CreateProject(new ProjectInfoDTO()
            {
                NameProject = vm.ProjectName,
                StackTecnology = vm.Description,
                IsAgile = vm.IsAgile
            });

            Projects.Add(newProject);
        }

        public void RemoveProjectCommand(ProjectInfoDTO project)
        {
            _projectService.DeleteProject(project.ProjectInfoId);

            Projects.Remove(CurrentProject);
            CurrentProject = Projects.FirstOrDefault();
        }

        public void RenameProjectCommand(ProjectInfoDTO project)
        {
            var vm = new ProjectInfoViewModel(_windowManager, _uow, project.ProjectInfoId, false)
            {
                Description = project.StackTecnology, 
                ProjectName = project.NameProject, 
                IsAgile = project.IsAgile,
                IdProject = project.ProjectInfoId,
                IsScrum = !project.IsAgile
            };

            if (_windowManager.ShowDialog(vm) != true)
            {
                return;
            }

            project.StackTecnology = vm.Description;
            project.NameProject = vm.ProjectName;
            project.IsAgile = vm.IsAgile;
            _projectService.UpdateProject(project);

            foreach (var item in vm.Workers)
            {
                _projectsService.AddUserForProject(project.ProjectInfoId, item.UserId);
            }

            Projects.Clear();
            var projectsNames = _projectService.GetAllProjects().ToList();
            projectsNames.ForEach((x)=> Projects.Add(x));
            CurrentProject = Projects.First(x=> x.ProjectInfoId == project.ProjectInfoId);
        }

        public void OpenWorkersCommand()
        {
            var workers = new WorkersForProjectViewModel(_uow);

            if (_windowManager.ShowDialog(workers) != true)
            {
                return;
            }
        }

        public void OpenStatisticsCommand()
        {
            var statistic = new StatisticViewModel(_uow);

            if (_windowManager.ShowDialog(statistic) != true)
            {
                return;
            }
        }
    }
}
