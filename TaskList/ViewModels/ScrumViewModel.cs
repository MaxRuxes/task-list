﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.ViewModels.Helpers;

namespace TaskList.ViewModels
{
    public class ScrumViewModel : BaseProjectViewModel
    {
        public ScrumViewModel(IWindowManager windowManager, string connectionString)
        {
            WindowManager = windowManager;
            ConnectionString = connectionString;

            Mapper = new MapperConfiguration((cfg) => { }).CreateMapper();

            Uow = new DAL.Repositories.EfUnitOfWork(connectionString);

            TodoService = new TodoService(Uow);
            UserService = new UserService(Uow);
            ProjectService = new ProjectService(Uow);
        }

        public override void Dispose()
        {

        }
    }
}
