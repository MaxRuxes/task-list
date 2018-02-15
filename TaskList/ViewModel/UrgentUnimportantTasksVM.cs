using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;
using TaskList.Model;
using TaskList.View;
using TaskList.ViewModel.Abstract;

namespace TaskList.ViewModel
{
    public class UrgentUnimportantTasksVM : TaskType
    {
        public UrgentUnimportantTasksVM()
        {
            InitTaskType(@"\UrgentUnimportantTasks.xml", @"\UrgentUnimportantTasks(Deletes).txt");
        }
    }
}
