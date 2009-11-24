using System;
using System.Collections.Generic;
using System.Windows;

namespace VisualTreeSpy
{
    public class ControlsUnderPointEventArgs : EventArgs
    {
        public IEnumerable<UIElement> Controls { get; set; }
        public Point MousePosition { get; set; }
    }
}