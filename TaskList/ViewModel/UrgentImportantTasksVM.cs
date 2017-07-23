using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TaskList.Model;
using System.Windows.Input;

namespace TaskList.ViewModel
{
    public class UrgentImportantTasksViewModel : TaskAction
    {
        public override ICommand AddTask
        {
            get
            {
                return new DelegateCommand((o) =>
                {

                });
            }
        }

        public override ICommand CompleteTask
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
