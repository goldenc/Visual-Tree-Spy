
namespace SL30PropertyGrid
{
	#region Using Directives
	using System;

	#endregion

	#region EditorAttribute
	/// <summary>
	/// EditorAttribute
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class EditorAttribute : Attribute
	{
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="typeName">The AssemblyQualifiedName of the type that must inherit from <see cref="ValueEditorBase"/></param>
		public EditorAttribute(string typeName)
		{
			if (string.IsNullOrEmpty(typeName)) throw new ArgumentNullException("typeName");
			this.EditorTypeName = typeName;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="type">The type that must inherit from <see cref="ValueEditorBase"/></param>
		public EditorAttribute(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			this.EditorTypeName = type.AssemblyQualifiedName;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the Editors TypeName
		/// </summary>
		public string EditorTypeName { get; private set; }
		#endregion

		#region Overrides
		/// <summary>
		/// Checks for equality
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			EditorAttribute attribute = obj as EditorAttribute;
			return (((attribute != null) && (attribute.EditorTypeName == this.EditorTypeName)));
		}
		/// <summary>
		/// Gets the hashcode
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion
	}
	#endregion
}
