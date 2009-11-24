using System;
using System.Windows;
using System.Windows.Controls;
using ShaderEffectLibrary;

namespace VisualTreeSpy
{
    public partial class ControlsUnderPointList
    {
        public event Action<ControlListItem> ItemSelected;

        public void InvokeItemSelected(ControlListItem item)
        {
            Action<ControlListItem> handler = ItemSelected;
            if (handler != null) handler(item);
        }

        public ControlsUnderPointList()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button_MouseLeave(sender, null);
            InvokeItemSelected(((FrameworkElement)sender).DataContext as ControlListItem);
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var item = GetControlListItem(sender);
            if(item != null)
            {
                item.Effect = new InvertColorEffect();
            }
        }

        private UIElement GetControlListItem(object sender)
        {
            var button = sender as Button;
            if(button != null)
            {
                var item = button.DataContext as ControlListItem;
                if(item != null)
                {
                    return item.Tag as UIElement;
                }
            }

            return null;
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var item = GetControlListItem(sender);
            if (item != null)
            {
                item.Effect = null;
            }
        }
    }
}