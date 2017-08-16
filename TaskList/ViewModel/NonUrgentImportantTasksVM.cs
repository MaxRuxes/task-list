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
    public class NonUrgentImportantTasksVM : TaskType
    {
        public NonUrgentImportantTasksVM()
        {
            InitTaskType(@"\NonUrgentImportantTasks.xml", @"\NonUrgentImportantTasks(Deletes).txt");
        }
    }
}
