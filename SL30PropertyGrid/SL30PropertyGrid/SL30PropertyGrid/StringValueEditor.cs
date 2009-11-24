using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SL30PropertyGrid
{
	public class StringValueEditor : ValueEditorBase
	{
		TextBox txt;
	    private string _newText;

	    public StringValueEditor(PropertyGridLabel label, PropertyItem property)
			: base(label, property)
		{
			if (property.PropertyType == typeof(Char))
			{
				if ((char)property.Value == '\0')
					property.Value = "";
			}

			property.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(property_PropertyChanged);
			property.ValueError += new EventHandler<ExceptionEventArgs>(property_ValueError);


			txt = new TextBox();
			txt.Height = 20;
			if (null != property.Value)
				txt.Text = property.Value.ToString();
			//txt.IsReadOnly = !this.Property.CanWrite;
			txt.Foreground = this.Property.CanWrite ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Gray);
			txt.BorderThickness = new Thickness(0);
			txt.Margin = new Thickness(0);
            txt.LostFocus += new RoutedEventHandler(txt_LostFocus);

			if (this.Property.CanWrite)
				txt.TextChanged += new TextChangedEventHandler(Control_TextChanged);

			this.Content = txt;
			this.GotFocus += new RoutedEventHandler(StringValueEditor_GotFocus);
		}

        void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            if(_newText != null)
            {
                Property.Value = _newText;
            }
        }

		void property_ValueError(object sender, ExceptionEventArgs e)
		{
			MessageBox.Show(e.EventException.Message);
		}
		void property_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
			{
				if (null != this.Property.Value)
					txt.Text = this.Property.Value.ToString();
				else
					txt.Text = string.Empty;
			}

			if (e.PropertyName == "CanWrite")
			{
				if (!this.Property.CanWrite)
					txt.TextChanged -= new TextChangedEventHandler(Control_TextChanged);
				else
					txt.TextChanged += new TextChangedEventHandler(Control_TextChanged);
				txt.IsReadOnly = !this.Property.CanWrite;
				txt.Foreground = this.Property.CanWrite ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Gray);
			}
		}

		void StringValueEditor_GotFocus(object sender, RoutedEventArgs e)
		{
			this.txt.Focus();
		}

		void Control_TextChanged(object sender, TextChangedEventArgs e)
		{
		    _newText = txt.Text;
		}
	}
}
