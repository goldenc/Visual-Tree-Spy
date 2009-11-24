
namespace SL30PropertyGrid
{
	#region Using Directives
	using System;
	#endregion

	#region BrowsableAttribute
	/// <summary>
	/// Marks a type as being Browsable
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public sealed class BrowsableAttribute : Attribute
	{
		// Fields
		private bool browsable = true;
		public static readonly BrowsableAttribute Default = Yes;
		public static readonly BrowsableAttribute No = new BrowsableAttribute(false);
		public static readonly BrowsableAttribute Yes = new BrowsableAttribute(true);

		// Methods
		public BrowsableAttribute(bool browsable)
		{
			this.browsable = browsable;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			BrowsableAttribute attribute = obj as BrowsableAttribute;
			return ((attribute != null) && (attribute.Browsable == this.browsable));
		}

		public override int GetHashCode()
		{
			return this.browsable.GetHashCode();
		}

		public bool IsDefaultAttribute()
		{
			return this.Equals(Default);
		}

		// Properties
		public bool Browsable
		{
			get
			{
				return this.browsable;
			}
		}
	}
	#endregion
}
