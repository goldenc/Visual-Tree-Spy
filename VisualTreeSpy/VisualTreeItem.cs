using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace VisualTreeSpy
{
    public class VisualTreeItem : INotifyPropertyChanged
    {
        public DependencyObject Tag { get; set; }
        public ObservableCollection<VisualTreeItem> Children { get; set; }

        public VisualTreeItem()
        {
            Children = new ObservableCollection<VisualTreeItem>();
        }

        public bool ChildrenSet { get; set; }

        public override string ToString()
        {
            return Tag != null ? Tag.ToString() : "Loading";
        }

        public void OnChildrenUpdated()
        {
            ChildrenSet = true;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Children"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}