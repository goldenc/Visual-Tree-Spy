using System.Windows;

namespace VisualTreeSpy
{
    public class ControlListItem
    {
        public DependencyObject Tag { get; set; }
        public string Name
        {
            get
            {
                var fe = Tag as FrameworkElement;
                if(fe != null)
                {
                    return string.Format("{0} ({1})", fe.Name, Tag);
                }
                return Tag.ToString();
            }
        }
    }
}