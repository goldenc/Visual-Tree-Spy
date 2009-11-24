using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

//Inspired by the bloh entry http://www.dolittle.com/blogs/einar/archive/2008/01/13/missing-enum-getvalues-when-doing-silverlight-for-instance.aspx


namespace SL30PropertyGrid.Converters
{
	public static class EnumHelper
	{
		private static Dictionary<Type, EnumWrapper[]> _enumCache = new Dictionary<Type, EnumWrapper[]>();

		public static T[] GetValues<T>()
		{
			Type enumType = typeof(T);

			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
			}

			List<T> values = new List<T>();

			var fields = from field in enumType.GetFields()
						 where field.IsLiteral
						 select field;

			foreach (FieldInfo field in fields)
			{
				object value = field.GetValue(enumType);
				values.Add((T)value);
			}

			return values.ToArray();
		}
		public static List<object> GetValues(Type enumType)
		{
			if (!enumType.IsEnum)
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

			List<object> values = new List<object>();

			var fields = from field in enumType.GetFields()
						 where field.IsLiteral
						 select field;

			foreach (FieldInfo field in fields)
			{
				object value = field.GetValue(enumType);
				values.Add(value);
			}

			return values;
		}
		public static EnumWrapper[] GetValuesWrapped(Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
			}
			if (_enumCache.ContainsKey(enumType))
				return _enumCache[enumType];

			List<EnumWrapper> values = new List<EnumWrapper>();

			var fields = from field in enumType.GetFields()
						 where field.IsLiteral
						 select field;

			foreach (FieldInfo field in fields)
			{
				object value = field.GetValue(enumType);
				//values.Add(value);
				values.Add(new EnumWrapper { Name = value.ToString(), Value = value });
			}
			EnumWrapper[] ret = values.ToArray();
			_enumCache.Add(enumType, ret);
			return ret;
		}
		public static EnumWrapper GetValueWrapped(object o)
		{
			Type enumType = o.GetType();
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
			}

			EnumWrapper[] values = GetValuesWrapped(enumType);
			EnumWrapper v = values.FirstOrDefault(ew => ew.Value.Equals(o));
			return v;
		}
	}

	public class EnumWrapper
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public override string ToString()
		{
			return Name;
		}

		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}
		public override bool Equals(object obj)
		{
			return this.Value.Equals(obj);
		}
	}
}
