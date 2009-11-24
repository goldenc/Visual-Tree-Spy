using System;
using System.Globalization;

namespace SL30PropertyGrid.Converters
{
	public class DoubleConverter : BaseNumberConverter
	{
		// Methods
		internal override object FromString(string value, CultureInfo culture)
		{
			return double.Parse(value, culture);
		}

		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return double.Parse(value, NumberStyles.Float, (IFormatProvider)formatInfo);
		}

		internal override object FromString(string value, int radix)
		{
			return Convert.ToDouble(value, CultureInfo.CurrentCulture);
		}

		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			double num = (double)value;
			return num.ToString("R", formatInfo);
		}

		// Properties
		internal override bool AllowHex
		{
			get
			{
				return false;
			}
		}

		internal override Type TargetType
		{
			get
			{
				return typeof(double);
			}
		}
	}
}
