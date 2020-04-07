using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TaskList.BLL.DTO;
using TaskList.BLL.Services;
using TaskList.Models;
using TaskList.ViewModels.Helpers;

namespace TaskList.ViewModels
{
    public class ScrumViewModel : BaseProjectViewModel
    {
        public ScrumViewModel(IWindowManager windowManager, string connectionString)
        {
            WindowManager = windowManager;
            ConnectionString = connectionString;

            Mapper = MapperHelpers.CreateAutoMapper();

            Uow = new DAL.Repositories.EfUnitOfWork(connectionString);

            TodoService = new TodoService(Uow);
            UserService = new UserService(Uow);
            ProjectService = new ProjectService(Uow);

            Login = connectionString
                .Split(';')
                .FirstOrDefault(n => n.IndexOf("uid=", StringComparison.Ordinal) != -1)?
                .Substring(4);
            CurrentUser = Mapper.Map<UserDTO, UserModel>(UserService.GetUser(1));
            SignInTime = DateTime.Now.ToUniversalTime().ToLongDateString() + DateTime.Now.ToShortTimeString();
        }

        public override void Dispose()
        {

        }
    }
}
