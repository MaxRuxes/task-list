using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TaskList.DAL.Entities;

namespace TaskList.ViewModels.Dialogs
{
    [Export(typeof(WorkersForProjectViewModel))]
    public class WorkersForProjectViewModel : Screen
    {
        public WorkersForProjectViewModel(IEnumerable<User> workers)
        {
            
        }
    }
}
