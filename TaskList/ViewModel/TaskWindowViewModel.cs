using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskList.ViewModel
{
    public class TaskWindowViewModel : ObservableObject
    {
        public string Content { get; private set; }

        public TaskWindowViewModel()
        {
            Content = "";
        }

        public DelegateCommand AcceptClick
        {
            get { return new DelegateCommand((o) => { ((Window)o).DialogResult = true; }); }
        }
    }
}
