using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TaskList.Model
{
    [Serializable]
    [XmlRoot("Tasks")]
    public class Data
    {
        public ObservableCollection<TheTask> Tasks { get; set; }
    }
}
