﻿using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Syncfusion.Windows.Shared;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.DAL.Interfaces;

namespace TaskList.ViewModels
{
    [Export(typeof(ProjectInfoViewModel))]
    public class ProjectInfoViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IUnitOfWork _uow;
        private string _description;
        private string _projectName;
        private bool _isAgile;
        private bool _isAddMode;
        private bool _isScrum;
        private UserDTO _selectedWorker;

        public ProjectInfoViewModel(IWindowManager windowManager, IUnitOfWork uow, int idProject, bool isAdd = true)
        {
            _windowManager = windowManager;
            _uow = uow;
            IsAddMode = isAdd;

            var projectsService = new ProjectsService(uow);
            var list = projectsService.GetAllUserForCurrentProject(idProject);
            Workers = new ObservableCollection<UserDTO>(list);
        }

        public int IdProject { get; set; }

        public bool IsAddMode
        {
            get => _isAddMode;
            set
            {
                _isAddMode = value;
                NotifyOfPropertyChange(() => _isAddMode);
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyOfPropertyChange(() => _description);
            }
        }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                NotifyOfPropertyChange(() => _projectName);
            }
        }

        public bool IsAgile
        {
            get => _isAgile;
            set
            {
                _isAgile = value;
                NotifyOfPropertyChange(() => _isAgile);
            }
        }

        public bool IsScrum
        {
            get => _isScrum;
            set
            {
                _isScrum = value;
                NotifyOfPropertyChange(() => _isScrum);
            }
        }

        public ObservableCollection<UserDTO> Workers { get; set; }

        public UserDTO SelectedWorker
        {
            get => _selectedWorker;
            set
            {
                _selectedWorker = value; 
                NotifyOfPropertyChange(()=>SelectedWorker);
                NotifyOfPropertyChange(()=>IsSelectedUser);
            }
        }

        public bool IsSelectedUser => SelectedWorker != null;

        public void SaveCommand()
        {
            if (ProjectName == null || Description == null ||
                ProjectName.Trim().IsNullOrWhiteSpace() || 
                ProjectName == "" ||
                Description.Trim().IsNullOrWhiteSpace() ||
                Description == "")
            {
                MessageBox.Show(
                    $"Проверьте правильность введенных данных.",
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                return;
            }

            if (Workers.Count == 0)
            {

                MessageBox.Show(
                    $"Выберите хотя бы одного сотрудника в проект. Если сотрудники отсутсвуют, вернитесь на начальную форму и" +
                    $" в соответствующем окне добавьте нового сотрудника.",
                    "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }


            TryClose(true);
        }

        public void CancelCommand()
        {
            TryClose(false);
        }

        public void AddWorkerCommand()
        {
            var workers = new WorkersSelectorViewModel(_uow, Workers);

            if (_windowManager.ShowDialog(workers) != true)
            {
                return;
            }

            if (Workers.Any(x => x.UserId == workers.SelectedWorker.UserId))
            {
                return;
            }

            Workers.Add(workers.SelectedWorker);
        }

        public void RemoveWorker()
        {
            Workers.Remove(SelectedWorker);
        }

    }
}

