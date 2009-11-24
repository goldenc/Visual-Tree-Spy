
namespace SL30PropertyGrid
{
	#region Using Directives
	using System.Windows.Controls;

	#endregion

	#region BooleanValueEditor
	/// <summary>
	/// An editor for a Boolean Type
	/// </summary>
	public class BooleanValueEditor : ComboBoxEditorBase
	{
		public BooleanValueEditor(PropertyGridLabel label, PropertyItem property)
			: base(label, property)
		{
		}
		public override void InitializeCombo()
		{
			this.LoadItems(new object[] { true, false });
		}
	}
	#endregion
}
