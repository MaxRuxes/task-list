using Caliburn.Micro;
using System.Collections.ObjectModel;

namespace TaskList.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        public MainWindowViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                CarouselItems.Add($"item{i.ToString()}");
            }
        }

        public ObservableCollection<string> CarouselItems { get; set; } = new ObservableCollection<string>();
    }
}
