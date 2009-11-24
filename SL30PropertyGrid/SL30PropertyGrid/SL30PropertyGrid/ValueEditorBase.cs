using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SL30PropertyGrid
{
	public abstract class ValueEditorBase : ContentControl, IPropertyValueEditor
	{
	    public event EventHandler RecurseProperties;

	    public void InvokeRecurseProperties(EventArgs e)
	    {
	        EventHandler handler = RecurseProperties;
	        if (handler != null) handler(this, e);
	    }

	    /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="label">The associated label for this Editor control</param>
		/// <param name="property">The associated PropertyItem for this control</param>
		public ValueEditorBase(PropertyGridLabel label, PropertyItem property)
		{
			this.Label = label;
			this.Label.Name = "lbl" + property.Name;
			this.Label.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(Label_MouseLeftButtonDown);
			this.Label.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(Label_MouseLeftButtonUp);
			if (!property.CanWrite)
				this.Label.Foreground = new SolidColorBrush(Colors.Gray);

			this.Name = "txt" + property.Name;
			this.Property = property;
			this.BorderThickness = new Thickness(0);
			this.Margin = new Thickness(0);
			this.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if (null == Label)
				return;

			base.OnGotFocus(e);

		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if (null == Label)
				return;

			base.OnLostFocus(e);

			if (this.IsSelected)
				this.Label.Background = new SolidColorBrush(PropertyGrid.backgroundColor);
			else
				this.Label.Background = new SolidColorBrush(Colors.White);

			if (this.Property.CanWrite)
				this.Label.Foreground = new SolidColorBrush(Colors.Black);
			else
				this.Label.Foreground = new SolidColorBrush(Colors.Gray);
		}

		private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			e.Handled = true;
		}
		private void Label_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.Focus();
		}


		/// <summary>
		/// Gets or sets whether this item is selected
		/// </summary>
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;

					if (value)
					{
						this.Label.Background = new SolidColorBrush(PropertyGrid.backgroundColorFocused);
						this.Label.Foreground = new SolidColorBrush(Colors.White);
					}
					else
					{
						this.Label.Background = new SolidColorBrush(Colors.White);
						if (this.Property.CanWrite)
							this.Label.Foreground = new SolidColorBrush(Colors.Black);
						else
							this.Label.Foreground = new SolidColorBrush(Colors.Gray);
					}
				}
			}
		} bool _isSelected;

		#region IPropertyValueEditor Members
		/// <summary>
		/// Gets the associated label for this Editor control
		/// </summary>
		public PropertyGridLabel Label { get; private set; }
		/// <summary>
		/// Gets the associated PropertyItem for this control
		/// </summary>
		public PropertyItem Property { get; private set; }
		#endregion
	}
}
