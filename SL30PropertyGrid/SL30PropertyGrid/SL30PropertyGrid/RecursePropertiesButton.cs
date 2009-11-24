using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SL30PropertyGrid
{
    public class RecursePropertiesButton : ValueEditorBase
    {
        public RecursePropertiesButton(PropertyGridLabel label, PropertyItem property)
            : base(label, property)
        {

            var button = new Button();
            button.Margin = new Thickness(0);
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.Content = "Edit Properties";
            button.Visibility = property.Value == null ? System.Windows.Visibility.Collapsed : Visibility.Visible;
            button.Click += (s, e) => InvokeRecurseProperties(EventArgs.Empty);

            var grid = new Grid();
            grid.Children.Add(button);

            this.Content = grid;   
        }
    }
}
