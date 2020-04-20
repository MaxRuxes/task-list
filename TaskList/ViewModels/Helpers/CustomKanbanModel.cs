using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.UI.Xaml.Kanban;
using TaskList.BLL.DTO;

namespace TaskList.ViewModels.Helpers
{
    public class CustomKanbanModel : KanbanModel
    {
        private TodoDTO _currentTodo;

        public CustomKanbanModel(TodoDTO todo)
        {
            CurrentTodo = todo;
        }

        public TodoDTO CurrentTodo { get; set; }
        public UserDTO CurrentUser { get; set; }
    }
}
