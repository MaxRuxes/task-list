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
using TaskList.ViewModels.Dialogs;

namespace TaskList.ViewModels
{
    [Export(typeof(ProjectsViewModel))]
    public class ProjectsViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly string _connectionString;

        private readonly IProjectService _projectService;

        public ProjectsViewModel(IWindowManager windowManager)
        {

            var connectionString = $"Server=127.0.0.1;database=mydb;uid=root;pwd=1234;SslMode=Required;Allow Zero Datetime=true";
            var connection = new MySqlConnection(connectionString);


            _windowManager = windowManager;
            _connectionString = connectionString;

            IUnitOfWork uow = new DAL.Repositories.EfUnitOfWork(connectionString);
            _projectService = new ProjectService(uow);

            var projectsNames = _projectService.GetAllProjects().ToList();

            Projects = new ObservableCollection<ProjectInfoDTO>(projectsNames);
            CurrentProject = Projects.FirstOrDefault();
        }

        public ObservableCollection<ProjectInfoDTO> Projects { get; set; }

        public ProjectInfoDTO CurrentProject { get; set; }

        public void SelectProject(ProjectInfoDTO project)
        {
            if (CurrentProject.IsAgile)
            {
                _windowManager.ShowWindow(new MainWindowViewModel(_windowManager, _connectionString)
                {
                    CurrentProject = project
                });
            }
            else
            {
                _windowManager.ShowDialog(new ScrumViewModel(_windowManager, _connectionString)
                {
                    CurrentProject = project
                });
            }

            (GetView() as Window)?.Close();
        }

        public void CreateProjectCommand()
        {
            var vm = new ProjectInfoViewModel();
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
            var vm = new ProjectInfoViewModel(false);
            vm.Description = project.StackTecnology;
            vm.ProjectName = project.NameProject;
            vm.IsAgile = project.IsAgile;

            if (_windowManager.ShowDialog(vm) != true)
            {
                return;
            }

            project.StackTecnology = vm.Description;
            project.NameProject = vm.ProjectName;
            project.IsAgile = vm.IsAgile;
            _projectService.UpdateProject(project);

            Projects.Clear();
            var projectsNames = _projectService.GetAllProjects().ToList();
            projectsNames.ForEach((x)=> Projects.Add(x));
            CurrentProject = Projects.First(x=> x.ProjectInfoId == project.ProjectInfoId);
        }
    }
}
