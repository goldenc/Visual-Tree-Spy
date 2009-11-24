using System;
using System.Globalization;

namespace SL30PropertyGrid.Converters
{
	public class ByteConverter : BaseNumberConverter
	{
		// Methods
		internal override object FromString(string value, CultureInfo culture)
		{
			return byte.Parse(value, culture);
		}

		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return byte.Parse(value, NumberStyles.Integer, (IFormatProvider)formatInfo);
		}

		internal override object FromString(string value, int radix)
		{
			return Convert.ToByte(value, radix);
		}

		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			byte num = (byte)value;
			return num.ToString("G", formatInfo);
		}

		// Properties
		internal override Type TargetType
		{
			get
			{
				return typeof(byte);
			}
		}
	}
}
