using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TaskList.BLL.DTO;
using TaskList.BLL.Interfaces;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Entities;
using TaskList.DAL.Repositories;

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

        public void CreateTodo(int idProject, TodoDTO todo, UserDTO owner)
        {
            var todoItem = _database.Todos.Get(todo.TodoId);
            if (todoItem != null)
            {
                throw new ArgumentException(@"Current todo already exists", nameof(todo));
            }

            var item =_database.Todos.Create(_mapper.Map<TodoDTO, Todo>(todo));

            _database.TodoAndUsers.Create(new TodoAndUsers { Iduser = owner.UserId, IdTodo = todo.TodoId });
            _database.TodoAndProjects.Create(new TodoAndProjects {IdProject = idProject, IdTodo = item.TodoId});

            _database.Save();
        }

        public void DeleteTodo(int idTodo, int idProject)
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

            // удаляем из связанных таблиц
            var todosInProjectsList = _database.TodoAndProjects
                .Find(o => o.IdTodo == idTodo && idProject == o.IdProject);
            if (todosInProjectsList != null)
            {
                foreach (var item in todosInProjectsList)
                {
                    _database.TodoAndProjects.Delete(item.TodoAndProjectsId);
                }
            }

            _database.Save();
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public int GetCountForProject(int currentProjectProjectInfoId)
        {
            return ((TodoAndProjectsRepository) _database.TodoAndProjects).GetCountForProject(
                currentProjectProjectInfoId);
        }

        public IEnumerable<TodoDTO> GetAllTodo(int idProject)
        {
            var items = _database.TodoAndProjects
                .Find(o => o.IdProject == idProject)
                .Select(x => x.IdTodo);


            var todos = _database.Todos
                .Find(x => items.Contains(x.TodoId));
            return _mapper
                .Map<IEnumerable<Todo>, List<TodoDTO>>(todos);
        }

        public IEnumerable<TodoDTO> GetAllTodosForProject(int idPrior, int idProject)
        {
            var items = _database.TodoAndProjects
                .Find(o => o.IdProject == idProject)
                .Select(x=> x.IdTodo);


            var todos = _database.Todos
                .Find(x => items.Contains(x.TodoId) && x.IdPriority == idPrior);
            return _mapper
                .Map<IEnumerable<Todo>, List<TodoDTO>>(todos);
        }

        public TodoDTO GetTodo(int idTodo)
        {
            return _mapper.Map<Todo, TodoDTO>(_database.Todos.Get(idTodo));
        }

        public void UpdateTodo(TodoDTO todo, UserDTO owner)
        {
            var old = _database.TodoAndUsers.Find(x => x.IdTodo == todo.TodoId);
            foreach (var todoAndUserse in old)
            {
                todoAndUserse.Iduser = owner.UserId;
                _database.TodoAndUsers.Update(todoAndUserse);
            }

            _database.Todos.Update(_mapper.Map<TodoDTO, Todo>(todo));
            _database.Save();
        }
    }
}
