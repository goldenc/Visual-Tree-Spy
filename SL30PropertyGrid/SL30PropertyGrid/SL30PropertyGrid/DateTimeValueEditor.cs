using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SL30PropertyGrid
{
	public class DateTimeValueEditor : ValueEditorBase
	{
		#region Fields
		object currentValue;
		bool showingDTP;
		StackPanel pnl;
		protected TextBox txt;
		protected DatePicker dtp;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="label"></param>
		/// <param name="property"></param>
		public DateTimeValueEditor(PropertyGridLabel label, PropertyItem property)
			: base(label, property)
		{
			currentValue = property.Value;
			property.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(property_PropertyChanged);
			property.ValueError += new EventHandler<ExceptionEventArgs>(property_ValueError);

			pnl = new StackPanel();
			this.Content = pnl;

			dtp = new DatePicker();
			dtp.Visibility = Visibility.Visible;
			dtp.Margin = new Thickness(0);
			dtp.VerticalAlignment = VerticalAlignment.Center;
			dtp.HorizontalAlignment = HorizontalAlignment.Stretch;
			dtp.CalendarOpened += new RoutedEventHandler(dtp_CalendarOpened);
			dtp.CalendarClosed += new RoutedEventHandler(dtp_CalendarClosed);
			dtp.LostFocus += new RoutedEventHandler(dtp_LostFocus);
			pnl.Children.Add(dtp);
			dtp.Focus();

			this.ShowTextBox();
		}
		#endregion

		#region Overrides
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			Debug.WriteLine("DateTimeValueEditor : OnGotFocus");

			if (showingDTP)
				return;

			base.OnGotFocus(e);

			if (this.Property.CanWrite)
				this.ShowDatePicker();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			Debug.WriteLine("DateTimeValueEditor : OnLostFocus");

			if (showingDTP)
				return;

			base.OnLostFocus(e);
		}
		#endregion

		#region Methods
		void ShowDatePicker()
		{
			if (null == txt)
				return;

			dtp.SelectedDateChanged -= new EventHandler<SelectionChangedEventArgs>(dtp_SelectedDateChanged);
			dtp.Visibility = Visibility.Visible;
			dtp.Focus();

			txt.Visibility = Visibility.Collapsed;
			pnl.Children.Remove(txt);
			txt = null;

			dtp.SelectedDate = (DateTime)currentValue;
			dtp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtp_SelectedDateChanged);

		}
		void ShowTextBox()
		{
			if (null != txt)
				return;

			txt = new TextBox();
			txt.Height = 20;
			txt.BorderThickness = new Thickness(0);
			txt.Margin = new Thickness(0);
			txt.VerticalAlignment = VerticalAlignment.Center;
			txt.HorizontalAlignment = HorizontalAlignment.Stretch;
			txt.Text = currentValue.ToString();
			txt.IsReadOnly = !this.Property.CanWrite;
			txt.Foreground = this.Property.CanWrite ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Gray);
			txt.Text = ((DateTime)this.Property.Value).ToShortDateString();
			pnl.Children.Add(txt);

			showingDTP = false;
			dtp.Visibility = Visibility.Collapsed;
		}
		#endregion

		#region Event Handlers
		void property_ValueError(object sender, ExceptionEventArgs e)
		{
			MessageBox.Show(e.EventException.Message);
		}
		void property_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
				currentValue = this.Property.Value;

			if (e.PropertyName == "CanWrite")
			{
				if (!this.Property.CanWrite && showingDTP)
					ShowTextBox();
			}
		}
		void dtp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			currentValue = e.AddedItems[0];
			this.Property.Value = currentValue;
		}
		void dtp_CalendarOpened(object sender, RoutedEventArgs e)
		{
			showingDTP = true;
		}
		void dtp_CalendarClosed(object sender, RoutedEventArgs e)
		{
			dtp.Focus();
		}
		void dtp_LostFocus(object sender, RoutedEventArgs e)
		{
			currentValue = dtp.SelectedDate;
			this.Property.Value = currentValue;
			if (dtp.IsDropDownOpen)
				return;
			ShowTextBox();
		}
		#endregion
	}
}
