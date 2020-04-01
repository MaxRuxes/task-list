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
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public TodoService(IUnitOfWork uow)
        {
            _database = uow;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TodoDTO, Todo>();
                cfg.CreateMap<Todo, TodoDTO>();
            }).CreateMapper();
        }

        public void CreateTodo(int userId, int IdProject, TodoDTO todo)
        {
            var todoItem = _database.Todos.Get(todo.TodoId);
            if (todoItem != null)
            {
                throw new ValidationException("Current todo already exists", "");
            }

            var item =_database.Todos.Create(_mapper.Map<TodoDTO, Todo>(todo));

            _database.TodoAndUsers.Create(new TodoAndUsers() { Iduser = userId, IdTodo = todo.TodoId });
            _database.TodoAndProjects.Create(new TodoAndProjects() {IdProject = IdProject, IdTodo = item.TodoId});

            _database.Save();
        }

        public void DeleteTodo(int idTodo)
        {
            // чистим запись в главной таблице
            var todoItem = _database.Todos.Get(idTodo);
            if (todoItem != null)
            {
                _database.Todos.Delete(idTodo);
            }

            // удаляем из связанных таблиц
            var todosInUserList = _database.TodoAndUsers.Find(o => o.IdTodo == idTodo);
            if (todosInUserList != null)
            {
                foreach (var item in todosInUserList)
                {
                    _database.TodoAndUsers.Delete(item.TodoAndUsersId);
                }
            }

            _database.Save();
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public IEnumerable<TodoDTO> GetAllTodosForProject(int idPrior, int idProject)
        {
            return _mapper
                .Map<IEnumerable<Todo>, List<TodoDTO>>(_database
                .Todos
                .Find(o => o.IdPriority == idPrior));
        }

        public TodoDTO GetTodo(int idTodo)
        {
            return _mapper.Map<Todo, TodoDTO>(_database.Todos.Get(idTodo));
        }

        public void UpdateTodo(TodoDTO todo)
        {
            _database.Todos.Update(_mapper.Map<TodoDTO, Todo>(todo));
            _database.Save();
        }
    }
}
