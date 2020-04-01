using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace TaskList.ViewModels.Dialogs
{
    [Export(typeof(ProjectInfoViewModel))]
    public class ProjectInfoViewModel : Screen
    {
        private string _description;
        private string _projectName;
        private bool _isAgile;
        private bool _isAddMode;
        private bool _isScrum;

        public ProjectInfoViewModel(bool isAdd = true)
        {
            IsAddMode = isAdd;
            IsAgile = true;
            IsScrum = !IsAgile;
        }

        public bool IsAddMode
        {
            get => _isAddMode;
            set
            {
                _isAddMode = value;
                NotifyOfPropertyChange(()=>_isAddMode);
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyOfPropertyChange(()=> _description);
            }
        }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value; 
                NotifyOfPropertyChange(()=>_projectName);
            }
        }

        public bool IsAgile
        {
            get => _isAgile;
            set
            {
                _isAgile = value;
                NotifyOfPropertyChange(()=>_isAgile);
            }
        }

        public bool IsScrum
        {
            get => _isScrum;
            set
            {
                _isScrum = value;
                NotifyOfPropertyChange(()=>_isScrum);
            }
        }

        public void SaveCommand()
        {
            TryClose(true);
        }

        public void CancelCommand()
        {
            TryClose(false);
        }
    }
}
