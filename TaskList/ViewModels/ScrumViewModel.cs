using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TaskList.BLL.DTO;

namespace TaskList.ViewModels
{
    public class ScrumViewModel
    {
        public ScrumViewModel(IWindowManager windowManager, string connectionString)
        {
            

        }

        public ProjectInfoDTO CurrentProject { get; set; }
    }
}
