using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskList.ToolKit.Command;
using TaskList.ToolKit.ViewModel;

namespace TaskList.ViewModel
{
    public class TaskWindowViewModel : ObservableObject
    {
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChangedEvent(nameof(Content)); }
        }

        public DelegateCommand AcceptClick
        {
            get
            {
                return new DelegateCommand((o) => { ((Window)o).DialogResult = true; });
            }
        }
    }
}
