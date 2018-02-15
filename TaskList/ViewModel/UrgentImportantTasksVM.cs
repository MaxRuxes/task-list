using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TaskList.Model;
using TaskList.View;
using TaskList.ViewModel.Abstract;

namespace TaskList.ViewModel
{
    public class UrgentImportantTasksVM : TaskType
    {
        public UrgentImportantTasksVM()
        {
            InitTaskType(@"\UrgentAndImportant.xml", @"\UrgentAndImportant(Deletes).txt");
        }
    }
}
