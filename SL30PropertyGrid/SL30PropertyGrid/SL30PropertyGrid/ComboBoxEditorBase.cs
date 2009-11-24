
namespace SL30PropertyGrid
{
	#region Using Directives
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	#endregion

	#region ComboBoxEditorBase
	/// <summary>
	/// An editor for a Boolean Type
	/// </summary>
	public abstract class ComboBoxEditorBase : ValueEditorBase
	{
		#region Fields
		object currentValue;
		bool showingCBO;
		StackPanel pnl;
		protected TextBox txt;
		protected ComboBox cbo;
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="label"></param>
		/// <param name="property"></param>
		public ComboBoxEditorBase(PropertyGridLabel label, PropertyItem property)
			: base(label, property)
		{

			currentValue = property.Value;
			property.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(property_PropertyChanged);
			property.ValueError += new EventHandler<ExceptionEventArgs>(property_ValueError);

			cbo = new ComboBox();
			cbo.Visibility = Visibility.Collapsed;
			cbo.Margin = new Thickness(0);
			cbo.VerticalAlignment = VerticalAlignment.Center;
			cbo.HorizontalAlignment = HorizontalAlignment.Stretch;
			cbo.DropDownOpened += new EventHandler(cbo_DropDownOpened);
			cbo.LostFocus += new RoutedEventHandler(cbo_LostFocus);
			this.InitializeCombo();

			pnl = new StackPanel();
			pnl.Children.Add(cbo);

			this.ShowTextBox();

			this.Content = pnl;
		}
		#endregion

		#region Overrides
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if (showingCBO)
				return;

			base.OnGotFocus(e);

			if (this.Property.CanWrite)
				this.ShowComboBox();

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if (cbo.IsDropDownOpen)
				return;

			base.OnLostFocus(e);
		}
		#endregion

		#region Methods
		void ShowComboBox()
		{
			if (null == txt)
				return;

			cbo.Visibility = Visibility.Visible;
			cbo.Focus();

			#region Sync current value
			this.cbo.SelectionChanged -= new SelectionChangedEventHandler(cbo_SelectionChanged);
			for (int i = 0; i < this.cbo.Items.Count; i++)
			{
				object val = this.cbo.Items[i];
				if (val.Equals(currentValue) || val.ToString() == currentValue.ToString())
				{
					this.cbo.SelectedIndex = i;
					break;
				}
			}
			this.cbo.SelectionChanged += new SelectionChangedEventHandler(cbo_SelectionChanged);
			#endregion

			txt.Visibility = Visibility.Collapsed;
			pnl.Children.Remove(txt);
			txt = null;
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
			txt.Text = currentValue.ToString();
			pnl.Children.Add(txt);

			cbo.Visibility = Visibility.Collapsed;
			showingCBO = false;

		}
		protected virtual void LoadItems(IEnumerable<object> items)
		{
			foreach (var item in items)
				this.cbo.Items.Add(item.ToString());
		}
		#endregion

		#region Abstract
		/// <summary>
		/// Initalize the combo box by calling LoadItems passing the list of items for the combobox
		/// </summary>
		public abstract void InitializeCombo();
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
				if (!this.Property.CanWrite && showingCBO)
					ShowTextBox();
			}
		}
		void cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			currentValue = e.AddedItems[0];
			this.Property.Value = currentValue;
		}
		void cbo_DropDownOpened(object sender, EventArgs e)
		{
			showingCBO = true;
		}
		void cbo_LostFocus(object sender, RoutedEventArgs e)
		{
			if (cbo.IsDropDownOpen)
				return;
			ShowTextBox();
		}
		#endregion
	}
	#endregion
}
