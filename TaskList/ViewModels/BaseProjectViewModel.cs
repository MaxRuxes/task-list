using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.Models;

namespace TaskList.ViewModels
{
    public abstract class BaseProjectViewModel : Screen, IDisposable
    {
        protected string SignInTime;

        protected IWindowManager WindowManager;
        protected string ConnectionString;
        protected IUnitOfWork Uow;
        protected IMapper Mapper;
        protected ITodoService TodoService;
        protected IUserService UserService;
        protected IProjectService ProjectService;
        protected UserModel CurrentUser;
        private TodoModel _selectedItem;
        private string _login;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                NotifyOfPropertyChange(() => Login);
            }
        }
        public ObservableCollection<TodoModel> TodoItems { get; set; } = new ObservableCollection<TodoModel>();

        public ProjectInfoDTO CurrentProject { get; internal set; }
        public string CountAllTodo => GetAllTodoForProjectCount().ToString();

        public TodoModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public string DateTimeSignIn => SignInTime;
        
        public abstract void Dispose();

        protected int GetAllTodoForProjectCount()
        {
            return TodoService.GetCountForProject(CurrentProject.ProjectInfoId);
        }

        public void OpenUserInfoWindow()
        {
            var projects = Mapper.Map<IEnumerable<ProjectInfoDTO>, List<ProjectModel>>(
                ProjectService.GetProjectsForUser(CurrentUser.UserId));

            WindowManager.ShowWindow(new UserInfoWindowViewModel(WindowManager, CurrentUser, projects));
        }

        public void BackToProjectsCommand()
        {
            WindowManager.ShowWindow(new ProjectsViewModel(WindowManager));
            (GetView() as Window)?.Close();
        }
    }
}
