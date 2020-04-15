using System.ComponentModel.Composition;
using Caliburn.Micro;
using TaskList.DAL.Interfaces;

namespace TaskList.ViewModels
{
    [Export(typeof(WorkersForProjectViewModel))]
    public class WorkersForProjectViewModel : Screen
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkersForProjectViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
