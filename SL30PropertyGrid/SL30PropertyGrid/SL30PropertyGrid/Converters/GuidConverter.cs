﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace SL30PropertyGrid.Converters
{
	public class GuidConverter : TypeConverter
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
				return new Guid(((string)value).Trim());
			}
			return base.ConvertFrom(context, culture, value);
		}


	}
}
