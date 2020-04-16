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
        private TodoModel _editTodoModel;
        protected IWindowManager WindowManager;
        protected string ConnectionString;
        protected IUnitOfWork Uow;
        protected IMapper Mapper;
        protected ITodoService TodoService;
        protected IUserService UserService;
        protected IProjectService ProjectService;
        private TodoModel _selectedItem;

        public ObservableCollection<TodoModel> TodoItems { get; set; } = new ObservableCollection<TodoModel>();

        public ProjectInfoDTO CurrentProject { get; internal set; }
        public string CountAllTodo => GetAllTodoForProjectCount().ToString();

        public TodoModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                EditTodoModel = value.Copy();
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public TodoModel EditTodoModel
        {
            get => _editTodoModel;
            set
            {
                _editTodoModel = value;
                NotifyOfPropertyChange(() => EditTodoModel);
            }
        }
        
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
    }
}
