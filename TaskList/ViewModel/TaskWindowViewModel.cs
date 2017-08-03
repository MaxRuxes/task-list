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
        private string _content;
        public string Content { get { return _content; }  set { _content = value; RaisePropertyChangedEvent(nameof(Content)); } }

        public DelegateCommand AcceptClick
        {
            get { return new DelegateCommand((o) => { ((Window)o).DialogResult = true; }); }
        }
    }
}
