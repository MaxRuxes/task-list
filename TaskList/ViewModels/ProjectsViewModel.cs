using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TaskList.BLL.Interfaces;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;
using TaskList.Models;

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

            Projects = new ObservableCollection<string>(projectsNames.Select(x => x.NameProject));
        }

        public ObservableCollection<string> Projects { get; set; }

        public void SelectProject(string project)
        {
            _windowManager.ShowWindow(new MainWindowViewModel(_windowManager, _connectionString) { CurrentProject = project});

        }

    }
}
