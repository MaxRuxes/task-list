using System;
using System.Linq;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Entities;
using TaskList.DAL.Interfaces;

namespace TaskList.BLL.Services
{
    public class TodoAndUsersService : ITodoAndUsersService
    {
        private readonly UserService _userService;
        private readonly IUnitOfWork _database;

        public TodoAndUsersService(IUnitOfWork uow)
        {
            _database = uow;
            _userService = new UserService(uow);
        }

        public UserDTO GetUserForTodo(int idTodo)
        {
            var todoAndUserItem = _database.TodoAndUsers.Find(x => idTodo == x.IdTodo).FirstOrDefault();
            if (todoAndUserItem == null)
            {
                return null;
            }

            var user =_userService.GetUser(todoAndUserItem.Iduser);
            return user;
        }

        public void CreateTodo(int userId, int idTodo)
        {
            var todoAndUserItem = _database.TodoAndUsers.Find(x => x.Iduser == userId && idTodo == x.IdTodo).Count();
            if (todoAndUserItem != 0)
            {
                throw new ArgumentException();
            }

            _database.TodoAndUsers.Create(new TodoAndUsers() {IdTodo = idTodo, Iduser = userId});
            _database.Save();
        }

        public void UpdateTodo(int idUser, int idTodo, int newIdUser)
        {
            var todoAndUserItem = _database.TodoAndUsers.Find(x => x.Iduser == idUser && idTodo == x.IdTodo).FirstOrDefault();
            if (todoAndUserItem == null)
            {
                throw new ArgumentException();
            }

            todoAndUserItem.Iduser = newIdUser;
            _database.TodoAndUsers.Update(todoAndUserItem);
            _database.Save();
        }

        public void DeleteTodo(int idTodo, int idUser)
        {
            var todoAndUserItem = _database.TodoAndUsers.Find(x => x.Iduser == idUser && idTodo == x.IdTodo).FirstOrDefault();
            if (todoAndUserItem == null)
            {
                return;
            }

            _database.TodoAndUsers.Delete(todoAndUserItem.TodoAndUsersId);
            _database.Save();
        }
    }
}
