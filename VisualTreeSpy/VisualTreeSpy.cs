using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace VisualTreeSpy
{
    public static class VisualTreeSpy
    {
        private static Popup _popup;
        private static ControlsUnderPointList _controlsList;

        public static void Start()
        {
            var trigger = new TrebleClickTrigger();
            var getControls = new GetControlsUnderPoint();
            getControls.ControlsFound += OnControlsFound;
            trigger.Actions.Add(getControls);

            Application.Current.RootVisual.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnLeftButtonDown), true);

            Interaction.GetTriggers(Application.Current.RootVisual).Add(trigger);
        }

        private static void OnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_popup != null && _popup.IsOpen)
            {
                _popup.IsOpen = false;
            }
        }

        private static void OnControlsFound(object sender, ControlsUnderPointEventArgs eventArgs)
        {
            if (_popup == null)
            {
                _popup = new Popup();
                _controlsList = new ControlsUnderPointList();
                _controlsList.ItemSelected += ControlsListItemSelected;
                _popup.Child = _controlsList;
            }

            _controlsList.list.ItemsSource = eventArgs.Controls.Select(c => new ControlListItem { Tag = c }).ToList();
            _popup.VerticalOffset = eventArgs.MousePosition.Y;
            _popup.HorizontalOffset = eventArgs.MousePosition.X;
            _popup.IsOpen = true;
        }

        static void ControlsListItemSelected(ControlListItem obj)
        {
            _popup.IsOpen = false;
            var spy = new SilverlightSpy();
            spy.TargetElement = obj.Tag;
            spy.Show();

        }
    }
}