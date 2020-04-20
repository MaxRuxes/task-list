using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface ITodoAndUsersService
    {
        UserDTO GetUserForTodo(int idTodo);
        void CreateTodo(int userId, int idTodo);
        void UpdateTodo(int idUser, int idTodo, int newIdUser);
        void DeleteTodo(int idTodo, int idUser);
    }
}
