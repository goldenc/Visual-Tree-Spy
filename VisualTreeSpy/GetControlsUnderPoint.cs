using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace VisualTreeSpy
{
    public class GetControlsUnderPoint : TriggerAction<UIElement>
    {
        public event EventHandler<ControlsUnderPointEventArgs> ControlsFound;

        protected override void Invoke(object parameter)
        {
            var eventArgs = parameter as MouseButtonEventArgs;
            if(eventArgs != null)
            {
                var controls = VisualTreeHelper.FindElementsInHostCoordinates(eventArgs.GetPosition(AssociatedObject),
                                                                              AssociatedObject);
                OnControlsFound(controls, eventArgs.GetPosition(AssociatedObject));
            }
        }

        protected void OnControlsFound(IEnumerable<UIElement> controls, Point point)
        {
            var temp = ControlsFound;
            if (temp != null)
            {
                temp(this, new ControlsUnderPointEventArgs {Controls = controls, MousePosition = point});
            }
        }
    }
}