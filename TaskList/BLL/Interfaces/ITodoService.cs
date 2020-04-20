using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface ITodoService
    {
        void CreateTodo(int idProject, TodoDTO todo, UserDTO owner);
        void UpdateTodo(TodoDTO todo, UserDTO owner);
        void DeleteTodo(int idTodo, int idProject);
        TodoDTO GetTodo(int idTodo);
        IEnumerable<TodoDTO> GetAllTodosForProject(int idPriority, int idProject);

        void Dispose();
        int GetCountForProject(int currentProjectProjectInfoId);
        IEnumerable<TodoDTO> GetAllTodo(int idProject);
    }
}
