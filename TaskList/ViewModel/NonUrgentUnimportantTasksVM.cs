using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;
using TaskList.Model;
using TaskList.View;

namespace TaskList.ViewModel
{
    public class NonUrgentUnimportantTasksVM : TaskType
    {
        public NonUrgentUnimportantTasksVM()
        {
            InitTaskType(@"\NonUrgentUnimportantTasks.xml", @"\NonUrgentUnimportantTasks(Deletes).txt");
        }
    }
}
