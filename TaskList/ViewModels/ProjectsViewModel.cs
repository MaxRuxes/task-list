using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
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

        private readonly IUnitOfWork uow;
        private readonly IProjectService projectService;

        public ProjectsViewModel(IWindowManager windowManager, string connectionString)
        {
            _windowManager = windowManager;
            _connectionString = connectionString;

            uow = new DAL.Repositories.EFUnitOfWork(connectionString);
            projectService = new ProjectService(uow);

            var projectsNames = projectService.GetAllProjects().ToList();

            Projects = new ObservableCollection<ProjectInfoDTO>(projectsNames);
            CurrentProject = Projects.FirstOrDefault();
        }

        public ObservableCollection<ProjectInfoDTO> Projects { get; set; }

        public ProjectInfoDTO CurrentProject { get; set; }

        public void SelectProject(string project)
        {
            _windowManager.ShowWindow(new MainWindowViewModel(_windowManager, _connectionString) { CurrentProject = project});
            (GetView() as Window)?.Close();
        }

        public void CreateProjectCommand(string project)
        {
            _windowManager.ShowWindow(new MainWindowViewModel(_windowManager, _connectionString) { CurrentProject = project});
            (GetView() as Window)?.Close();
        }

        public void RemoveProjectCommand(ProjectInfoDTO project)
        {
            projectService.DeleteProject(project.ProjectInfoId);

            Projects.Remove(CurrentProject);
            CurrentProject = Projects.FirstOrDefault();
        }

        public void RenameProjectCommand(string project)
        {
            _windowManager.ShowWindow(new MainWindowViewModel(_windowManager, _connectionString) { CurrentProject = project});
            (GetView() as Window)?.Close();
        }





    }
}
