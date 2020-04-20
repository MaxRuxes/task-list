using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using Caliburn.Micro;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.ViewModels.Models;

namespace TaskList.ViewModels
{
    public abstract class BaseProjectViewModel : Screen, IDisposable
    {
        private TodoModel _editTodoModel;
        protected IWindowManager WindowManager;
        protected string ConnectionString;
        protected IUnitOfWork Uow;
        protected IMapper Mapper;
        protected ITodoService TodoService;
        protected IUserService UserService;
        protected IProjectService ProjectService;
        protected ITodoAndUsersService TodoAndUsersService;
        private TodoModel _selectedItem;
        private bool _isEditNow;

        protected BaseProjectViewModel(ProjectInfoDTO project)
        {
            CurrentProject = project;
        }

        public ObservableCollection<TodoModel> TodoItems { get; set; } = new ObservableCollection<TodoModel>();

        public ProjectInfoDTO CurrentProject { get; internal set; }
        public string CountAllTodo => GetAllTodoForProjectCount().ToString();


        public bool IsEditNow
        {
            get => _isEditNow;
            set
            {
                _isEditNow = value;
                NotifyOfPropertyChange(() => IsEditNow);
            }
        }

        public bool IsEditTodoModelEnabled => EditTodoModel != null;

        public TodoModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                EditTodoModel = value;
                if (value != null)
                {
                    EditTodoModel = value.Copy();
                }

                NotifyOfPropertyChange(() => SelectedItem);
                NotifyOfPropertyChange(()=>EditTodoModel);
            }
        }

        public TodoModel EditTodoModel
        {
            get => _editTodoModel;
            set
            {
                _editTodoModel = value;
                NotifyOfPropertyChange(() => EditTodoModel);
                NotifyOfPropertyChange(() => IsEditTodoModelEnabled);
            }
        }

        #region Commands

        public ICommand StartExecuteCommand { get; set; }
        public ICommand EndExecuteCommand { get; set; }

        #endregion

        public abstract void Dispose();

        protected int GetAllTodoForProjectCount()
        {
            return TodoService.GetCountForProject(CurrentProject.ProjectInfoId);
        }

        public void BackToProjectsCommand()
        {
            WindowManager.ShowWindow(new ProjectsViewModel(WindowManager));
            (GetView() as Window)?.Close();
        }

        public void StartExecuteCommandExecute(object o)
        {
            EditTodoModel.StartDate = DateTime.Today;
            EditTodoModel.State = 0;
            NotifyOfPropertyChange(() => EditTodoModel.State);
            Refresh();
        }

        public void EndExecuteCommandExecute(object o)
        {
            EditTodoModel.State = 1;
            NotifyOfPropertyChange(() => EditTodoModel.State);

            EditTodoModel.EndRealDate = DateTime.Today;
            NotifyOfPropertyChange(() => EditTodoModel.EndRealDate);

            var days = (EditTodoModel.EndRealDate - EditTodoModel.StartDate).Days;
            days = days == 0 ? 1 : days;
            EditTodoModel.SpentTime = days * 8;
            NotifyOfPropertyChange(() => EditTodoModel.SpentTime);
            NotifyOfPropertyChange(() => EditTodoModel);
            Refresh();
        }
    }
}
