using System;
using System.Globalization;

namespace SL30PropertyGrid.Converters
{
	public class DecimalConverter : BaseNumberConverter
	{

		internal override object FromString(string value, CultureInfo culture)
		{
			return decimal.Parse(value, culture);
		}

		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return decimal.Parse(value, NumberStyles.Float, formatInfo);
		}

		internal override object FromString(string value, int radix)
		{
			return Convert.ToDecimal(value, CultureInfo.CurrentCulture);
		}

		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			decimal num = (decimal)value;
			return num.ToString("G", formatInfo);
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
				return typeof(decimal);
			}
		}
	}
}
