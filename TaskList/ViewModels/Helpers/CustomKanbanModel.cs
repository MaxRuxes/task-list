using Syncfusion.UI.Xaml.Kanban;
using TaskList.BLL.DTO;

namespace TaskList.ViewModels.Helpers
{
    public class CustomKanbanModel : KanbanModel
    {
        public CustomKanbanModel(TodoDTO todo)
        {
            CurrentTodo = todo;
        }

        public TodoDTO CurrentTodo { get; set; }
        public UserDTO CurrentUser { get; set; }
    }
}
