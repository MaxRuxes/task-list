using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface ITodoService
    {
        void CreateTodo(int userId, int idProject, TodoDTO todo);
        void UpdateTodo(TodoDTO todo);
        void DeleteTodo(int idTodo, int idProject);
        TodoDTO GetTodo(int idTodo);
        IEnumerable<TodoDTO> GetAllTodosForProject(int idPriority, int idProject);

        void Dispose();
        int GetCountForProject(int currentProjectProjectInfoId);
    }
}
