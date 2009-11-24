using System;
using System.ComponentModel;
using System.Globalization;

namespace SL30PropertyGrid.Converters
{
	public class TimeSpanConverter : TypeConverter
	{
		// Methods
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string s = ((string)value).Trim();
				try
				{
					return TimeSpan.Parse(s);
				}
				catch (FormatException)
				{
					throw new FormatException();
				}
			}
			return base.ConvertFrom(context, culture, value);
		}


	}
}
