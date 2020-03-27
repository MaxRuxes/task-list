using AutoMapper;
using System.Collections.Generic;
using TaskList.BLL.DTO;
using TaskList.BLL.Infrastructure;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;

namespace TaskList.BLL.Services
{
    public class TodoService : ITodoService
    {
        private IUnitOfWork Database { get; set; }
        private readonly IMapper mapper;

        public TodoService(IUnitOfWork uow)
        {
            Database = uow;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TodoDTO, Todo>();
                cfg.CreateMap<Todo, TodoDTO>();
            }).CreateMapper();
        }

        public void CreateTodo(int userId,TodoDTO todo)
        {
            var todoItem = Database.Todos.Get(todo.TodoId);
            if (todoItem != null)
            {
                throw new ValidationException("Current todo already exists", "");
            }

            if (todoItem == null)
            {
                Database.Todos.Create(mapper.Map<TodoDTO, Todo>(todo));
            }

            Database.TodoAndUsers.Create(new TodoAndUsers() { Iduser = userId, IdTodo = todo.TodoId });

            Database.Save();
        }

        public void DeleteTodo(int idTodo)
        {
            // чистим запись в главной таблице
            var todoItem = Database.Todos.Get(idTodo);
            if (todoItem != null)
            {
                Database.Todos.Delete(idTodo);
            }

            // удаляем из связанных таблиц
            var todosInUserList = Database.TodoAndUsers.Find(o => o.IdTodo == idTodo);
            if (todosInUserList != null)
            {
                foreach (var item in todosInUserList)
                {
                    Database.TodoAndUsers.Delete(item.TodoAndUsersId);
                }
            }

            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<TodoDTO> GetAllTodos(int idPrior)
        {
            return mapper
                .Map<IEnumerable<Todo>, List<TodoDTO>>(Database
                .Todos
                .Find(o => o.IdPriority == idPrior));
        }

        public TodoDTO GetTodo(int idTodo)
        {
            return mapper.Map<Todo, TodoDTO>(Database.Todos.Get(idTodo) ?? default);
        }

        public void UpdateTodo(TodoDTO todo)
        {
            Database.Todos.Update(mapper.Map<TodoDTO, Todo>(todo));
            Database.Save();
        }
    }
}
