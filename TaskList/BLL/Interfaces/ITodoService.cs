using System.Collections.Generic;
using TaskList.BLL.DTO;

namespace TaskList.BLL.Interfaces
{
    public interface ITodoService
    {
        void CreateTodo(int userId, TodoDTO todo);
        void UpdateTodo(TodoDTO todo);
        void DeleteTodo(int idTodo);
        TodoDTO GetTodo(int idTodo);
        IEnumerable<TodoDTO> GetAllTodos(int idPriority);

        void Dispose();
    }
}
