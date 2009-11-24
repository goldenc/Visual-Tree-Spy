using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace VisualTreeSpy
{
    public class TrebleClickTrigger : TriggerBase<UIElement>
    {
        private int _count;
        private readonly DispatcherTimer _timer;
        private Point _initialPoint;

        public TrebleClickTrigger()
        {
            _timer = new DispatcherTimer
                         {
                             Interval = TimeSpan.FromMilliseconds(550)
                         };

            _timer.Tick += OnTimerTick;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseButtonDown));

            if (_timer.IsEnabled)
            {
                StopTimer();
            }
        }

        private void StopTimer()
        {
            _count = 0;
            _timer.Stop();
        }

        private void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            _count++;

            if (!_timer.IsEnabled)
            {
                _initialPoint = e.GetPosition(AssociatedObject);
                _timer.Start();
                return;
            }

            if(!ClickWithinTenPixels(e.GetPosition(AssociatedObject)))
            {
                StopTimer();
            }

            if (_count == 3)
            {
                StopTimer();
                InvokeActions(e);
            }
        }



        private void OnTimerTick(object sender, EventArgs e)
        {
            StopTimer();
        }

        private bool ClickWithinTenPixels(Point location)
        {
            return Math.Abs(location.X - _initialPoint.X) < 10 && Math.Abs(location.Y - _initialPoint.Y) < 10;
        }

    }
}