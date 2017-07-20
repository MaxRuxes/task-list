using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TaskList.Model;

namespace TaskList.ViewModel
{
    public class TaskViewModel : ObservableObject
    {
        public ObservableCollection<TaskAction> Task1 { get; set; }

        public TaskViewModel()
        {
            StartTimer();
            Task1 = new ObservableCollection<TaskAction>();
            MessageBox.Show("Ефы");
            MessageBox.Show(System.Threading.Thread.CurrentThread.Name.ToString());
        }

        public void StartTimer()
        {
            Timer tm = new Timer(1000);
            tm.Elapsed += (e, sender) =>
            {
                Task1.Add(new TaskAction());
                MessageBox.Show("Действие");
            };
            tm.Start();
        }
    }
}
