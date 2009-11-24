
namespace SL30PropertyGrid
{
	#region Using Directives
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Browser;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	#endregion

	#region PropertyGrid
	/// <summary>
	/// PropertyGrid
	/// </summary>
	public partial class PropertyGrid : ContentControl
	{
		#region Fields

		//#99B4D1
		//internal static Color backgroundColor = Color.FromArgb(127, 153, 180, 209);

		//#E9ECFA
		internal static Color backgroundColor = Color.FromArgb(255, 233, 236, 250);
		internal static Color backgroundColorFocused = Color.FromArgb(255, 94, 170, 255);

		static Type thisType = typeof(PropertyGrid);

		ValueEditorBase selectedEditor;
		ScrollViewer LayoutRoot;
		Grid MainGrid;
        private Button PopButton;
		bool loaded = false;
		bool resetLoadedObject;

        private Stack<PropertyItem> _objectGraph = new Stack<PropertyItem>();

		#endregion

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		public PropertyGrid()
		{
			base.DefaultStyleKey = typeof(PropertyGrid);
			this.Loaded += new RoutedEventHandler(PropertyGrid_Loaded);
		}
		#endregion

		#region Properties

		#region SelectedObject

		public static readonly DependencyProperty SelectedObjectProperty =
		  DependencyProperty.Register("SelectedObject", typeof(object), thisType, new PropertyMetadata(null, OnSelectedObjectChanged));

		public object SelectedObject
		{
			get { return base.GetValue(SelectedObjectProperty); }
			set { base.SetValue(SelectedObjectProperty, value); }
		}

		private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = d as PropertyGrid;
			if (propertyGrid != null)
			{
				if (!propertyGrid.loaded)
					propertyGrid.resetLoadedObject = true;
				else if (null != e.NewValue)
					propertyGrid.ResetObject(e.NewValue);
				else
					propertyGrid.ResetMainGrid();
			}
		}
		#endregion

		#region Default LabelWidth
		/// <summary>
		/// The DefaultLabelWidth DependencyProperty
		/// </summary>
		public static readonly DependencyProperty DefaultLabelWidthProperty =
		  DependencyProperty.Register("DefaultLabelWidth", typeof(int), thisType, new PropertyMetadata(75));
		/// <summary>
		/// Gets or sets the Default Width for the labels
		/// </summary>
		public int DefaultLabelWidth
		{
			get { return (int)base.GetValue(DefaultLabelWidthProperty); }
			set { base.SetValue(DefaultLabelWidthProperty, value); }
		}
		#endregion

		#region Grid BorderBrush

		public static readonly DependencyProperty GridBorderBrushProperty =
			DependencyProperty.Register("GridBorderBrush", typeof(Brush), thisType, new PropertyMetadata(new SolidColorBrush(Colors.LightGray), OnGridBorderBrushChanged));

		/// <summary>
		/// Gets or sets the Border Brush of the Property Grid
		/// </summary>
		public Brush GridBorderBrush
		{
			get { return (Brush)base.GetValue(GridBorderBrushProperty); }
			set { base.SetValue(GridBorderBrushProperty, value); }
		}

		private static void OnGridBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = d as PropertyGrid;
			if (propertyGrid != null && null != propertyGrid.LayoutRoot && null != e.NewValue)
				propertyGrid.LayoutRoot.BorderBrush = (SolidColorBrush)e.NewValue;
		}

		#endregion

		#region Grid BorderThickness

		public static readonly DependencyProperty GridBorderThicknessProperty =
			DependencyProperty.Register("GridBorderThickness", typeof(Thickness), thisType, new PropertyMetadata(new Thickness(1), OnGridBorderThicknessChanged));


	    /// <summary>
		/// Gets or sets the Border Thickness of the Property Grid
		/// </summary>
		public Thickness GridBorderThickness
		{
			get { return (Thickness)base.GetValue(GridBorderThicknessProperty); }
			set { base.SetValue(GridBorderThicknessProperty, value); }
		}


		private static void OnGridBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PropertyGrid propertyGrid = d as PropertyGrid;
			if (propertyGrid != null && null != propertyGrid.LayoutRoot && null != e.NewValue)
				propertyGrid.LayoutRoot.BorderThickness = (Thickness)e.NewValue;
		}

		#endregion

		#endregion

		#region Overrides
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			this.LayoutRoot = (ScrollViewer)this.GetTemplateChild("LayoutRoot");
			this.MainGrid = (Grid)this.GetTemplateChild("MainGrid");
		    PopButton = (Button) GetTemplateChild("PopButton");
            PopButton.Click += new RoutedEventHandler(PopButton_Click);
		    PopButton.IsEnabled = false;

			loaded = true;

			if (resetLoadedObject)
			{
				resetLoadedObject = false;
				this.ResetObject(this.SelectedObject);
			}
		}

        void PopButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _objectGraph.Pop();
            item.Value = item.Value;
            SelectedObject = item.Instance;
            PopButton.IsEnabled = _objectGraph.Count > 0;
        }
		#endregion

		#region Methods

		int SetObject(object obj)
		{
			List<PropertyItem> props = new List<PropertyItem>();
			int rowCount = -1;

			// Parse the objects properties
			props = PropertyGrid.ParseObject(obj);

			#region Render the Grid

			var categories = (from p in props
							  orderby p.Category
							  select p.Category).Distinct();

			foreach (string category in categories)
			{

				this.AddHeaderRow(category, ref rowCount);

				var items = from p in props
							where p.Category == category
							orderby p.Name
							select p;

				foreach (var item in items)
					this.AddPropertyRow(item, ref rowCount);

			}
			#endregion

			return rowCount++;

		}

		void ResetObject(object obj)
		{
			this.ResetMainGrid();

			int rowCount = this.SetObject(obj);

			if (rowCount > 0)
				AddGridSplitter(rowCount);
		}
		void ResetMainGrid()
		{
			this.MainGrid.Children.Clear();
			this.MainGrid.RowDefinitions.Clear();
		}
		void AddHeaderRow(string category, ref int rowIndex)
		{
			rowIndex++;
			MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(21) });

			#region Column 0 - Margin

			Border brd = GetCategoryMargin(category, GetHideImage(Visibility.Visible), GetShowImage(Visibility.Collapsed));
			MainGrid.Children.Add(brd);
			Grid.SetRow(brd, rowIndex);
			Grid.SetColumn(brd, 0);

			#endregion

			#region Column 1 & 2 - Category Header

			brd = GetCategoryHeader(category);
			MainGrid.Children.Add(brd);
			Grid.SetRow(brd, rowIndex);
			Grid.SetColumn(brd, 1);
			Grid.SetColumnSpan(brd, 2);

			#endregion
		}
		void AddPropertyRow(PropertyItem item, ref int rowIndex)
		{
			#region Create Display Objects
			PropertyGridLabel label = CreateLabel(item.Name, item.DisplayName);
			ValueEditorBase editor = EditorService.GetEditor(item, label);
			if (null == editor)
				return;
			editor.GotFocus += new RoutedEventHandler(this.Editor_GotFocus);
            editor.RecurseProperties += new EventHandler(editor_RecurseProperties);
			#endregion

			rowIndex++;
			MainGrid.RowDefinitions.Add(new RowDefinition());
			string tagValue = item.Category;

			#region Column 0 - Margin
			Border brd = GetItemMargin(tagValue);
			MainGrid.Children.Add(brd);
			Grid.SetRow(brd, rowIndex);
			Grid.SetColumn(brd, 0);
			#endregion

			#region Column 1 - Label
			brd = GetItemLabel(label, tagValue);
			MainGrid.Children.Add(brd);
			Grid.SetRow(brd, rowIndex);
			Grid.SetColumn(brd, 1);
			#endregion

			#region Column 2 - Editor
			brd = GetItemEditor(editor, tagValue);
			MainGrid.Children.Add(brd);
			Grid.SetRow(brd, rowIndex);
			Grid.SetColumn(brd, 2);
			#endregion
		}

        void editor_RecurseProperties(object sender, EventArgs e)
        {
            var editor = sender as ValueEditorBase;
            if(editor != null)
            {
                _objectGraph.Push(editor.Property);
                SelectedObject = editor.Property.Value;
                PopButton.IsEnabled = true;
            }
        }
		void AddGridSplitter(int rowCount)
		{
			GridSplitter gsp = new GridSplitter()
			{
				IsTabStop = false,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Stretch,
				Background = new SolidColorBrush(Colors.Transparent),
				ShowsPreview = false,
				Width = 2
			};
			Grid.SetColumn(gsp, 2);
			Grid.SetRowSpan(gsp, rowCount);
			Canvas.SetZIndex(gsp, 1);
			MainGrid.Children.Add(gsp);

		}
		void ToggleCategoryVisible(bool show, string tagValue)
		{
			foreach (FrameworkElement element in this.MainGrid.Children)
			{
				object value = element.Tag;
				if (null != value)
				{
					string tag = (string)value;
					if (tagValue == tag)
						element.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
				}
			}
		}
		void AttachWheelEvents()
		{
			HtmlPage.Window.AttachEvent("DOMMouseScroll", OnMouseWheel);
			HtmlPage.Window.AttachEvent("onmousewheel", OnMouseWheel);
			HtmlPage.Document.AttachEvent("onmousewheel", OnMouseWheel);
		}
		void DetachWheelEvents()
		{
			HtmlPage.Window.DetachEvent("DOMMouseScroll", OnMouseWheel);
			HtmlPage.Window.DetachEvent("onmousewheel", OnMouseWheel);
			HtmlPage.Document.DetachEvent("onmousewheel", OnMouseWheel);
		}
		Image GetHideImage(Visibility visibility)
		{
			Image img = GetImage("assets/minus.png");
			img.Visibility = visibility;
			img.MouseLeftButtonUp += new MouseButtonEventHandler(this.CategoryHide_MouseLeftButtonUp);
			return img;
		}
		Image GetShowImage(Visibility visibility)
		{
			Image img = GetImage("assets/plus.png");
			img.Visibility = visibility;
			img.MouseLeftButtonUp += new MouseButtonEventHandler(this.CategoryShow_MouseLeftButtonUp);
			return img;
		}

		static List<PropertyItem> ParseObject(object objItem)
		{
			if (null == objItem)
				return new List<PropertyItem>();

			List<PropertyItem> pc = new List<PropertyItem>();
			Type t = objItem.GetType();
			var props = t.GetProperties();

			foreach (PropertyInfo pinfo in props)
			{
				bool isBrowsable = true;
				BrowsableAttribute b = PropertyItem.GetAttribute<BrowsableAttribute>(pinfo);
				if (null != b)
					isBrowsable = b.Browsable;
				if (isBrowsable)
				{
					EditorBrowsableAttribute eb = PropertyItem.GetAttribute<EditorBrowsableAttribute>(pinfo);
					if (null != eb && eb.State == EditorBrowsableState.Never)
						isBrowsable = false;
				}
				if (isBrowsable)
				{
					bool readOnly = false;
					ReadOnlyAttribute attr = PropertyItem.GetAttribute<ReadOnlyAttribute>(pinfo);
					if (attr != null)
						readOnly = attr.IsReadOnly;

					try
					{
						object value = pinfo.GetValue(objItem, null);
						PropertyItem prop = new PropertyItem(objItem, value, pinfo, readOnly);
						pc.Add(prop);
					}
					catch { }
				}
			}

			return pc;
		}
		static PropertyGridLabel CreateLabel(string name, string displayName)
		{
			TextBlock txt = new TextBlock()
			{
				Text = displayName,
				Margin = new Thickness(0)
			};
			return new PropertyGridLabel()
			{
				Name = Guid.NewGuid().ToString("N"),
				Content = txt
			};
		}
		static Border GetCategoryMargin(string tagValue, Image hide, Image show)
		{
			StackPanel stp = new StackPanel()
			{
				Name = Guid.NewGuid().ToString("N"),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			stp.Tag = tagValue;
			stp.Children.Add(hide);
			stp.Children.Add(show);

			Border brd = new Border() { Background = new SolidColorBrush(backgroundColor) };
			brd.Child = stp;

			return brd;
		}
		static Border GetCategoryHeader(string category)
		{
			TextBlock txt = new TextBlock()
			{
				Name = Guid.NewGuid().ToString("N"),
				Text = category,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left,
				Foreground = new SolidColorBrush(Colors.Gray),
				Margin = new Thickness(3, 0, 0, 0),
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Arial Narrow")
			};

			Border brd = new Border();
			brd.Background = new SolidColorBrush(backgroundColor);
			brd.Child = txt;
			Canvas.SetZIndex(brd, 1);
			return brd;
		}
		static Border GetItemMargin(string tagValue)
		{
			return new Border()
			{
				Name = Guid.NewGuid().ToString("N"),
				Margin = new Thickness(0),
				BorderThickness = new Thickness(0),
				Background = new SolidColorBrush(backgroundColor),
				Tag = tagValue
			};
		}
		static Border GetItemLabel(PropertyGridLabel label, string tagValue)
		{
			return new Border()
			{
				Name = Guid.NewGuid().ToString("N"),
				Margin = new Thickness(0),
				BorderBrush = new SolidColorBrush(backgroundColor),
				BorderThickness = new Thickness(0, 0, 1, 1),
				Child = label,
				Tag = tagValue
			};
		}
		static Border GetItemEditor(ValueEditorBase editor, string tagValue)
		{
			Border brd = new Border()
			{
				Name = Guid.NewGuid().ToString("N"),
				Margin = new Thickness(1, 0, 0, 0),
				BorderThickness = new Thickness(0, 0, 0, 1),
				BorderBrush = new SolidColorBrush(backgroundColor)
			};
			brd.Child = editor;
			brd.Tag = tagValue;
			return brd;
		}
		static Image GetImage(string imageUri)
		{
			//
			Image img = new Image()
			{
				Name = Guid.NewGuid().ToString("N"),
				Source = new BitmapImage(new Uri(imageUri, UriKind.Relative)),
				Height = 9,
				Width = 9,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			return img;
		}

		#endregion

		#region Event Handlers
		private void PropertyGrid_Loaded(object sender, RoutedEventArgs e)
		{
			this.MouseEnter += new MouseEventHandler(PropertyGrid_MouseEnter);
			this.MouseLeave += new MouseEventHandler(PropertyGrid_MouseLeave);
		}
		private void Editor_GotFocus(object sender, RoutedEventArgs e)
		{
			if (null != selectedEditor)
				selectedEditor.IsSelected = false;
			selectedEditor = sender as ValueEditorBase;
			if (null != selectedEditor)
			{
				selectedEditor.IsSelected = true;

				//double editorX = ((UIElement)selectedEditor.Parent).RenderTransformOrigin.X;
				//Debug.WriteLine("editorX: " + editorX.ToString());
				//double editorY = ((UIElement)selectedEditor.Parent).RenderTransformOrigin.Y;
				//Debug.WriteLine("editorY: " + editorY.ToString());

				//double thisWidth = this.RenderSize.Width;
				//Debug.WriteLine("thisWidth: " + thisWidth.ToString());
				//double thisHeight = this.RenderSize.Height;
				//Debug.WriteLine("thisHeight: " + thisHeight.ToString());

			}
		}
		private void PropertyGrid_MouseEnter(object sender, MouseEventArgs e)
		{
			this.AttachWheelEvents();
		}
		private void PropertyGrid_MouseLeave(object sender, MouseEventArgs e)
		{
			this.DetachWheelEvents();
		}
		private void OnMouseWheel(object sender, HtmlEventArgs args)
		{
			double mouseDelta = 0;
			ScriptObject e = args.EventObject;

			// Mozilla and Safari    
			if (e.GetProperty("detail") != null)
			{
				mouseDelta = ((double)e.GetProperty("detail"));
			}

				// IE and Opera    
			else if (e.GetProperty("wheelDelta") != null)
				mouseDelta = ((double)e.GetProperty("wheelDelta"));

			mouseDelta = Math.Sign(mouseDelta);
			mouseDelta = mouseDelta * -1;
			mouseDelta = mouseDelta * 40; // Just a guess at an acceleration
			mouseDelta = this.LayoutRoot.VerticalOffset + mouseDelta;
			this.LayoutRoot.ScrollToVerticalOffset(mouseDelta);
		}
		private void CategoryHide_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			FrameworkElement ctl = sender as FrameworkElement;
			Panel stp = ctl.Parent as Panel;
			string tagValue = (string)stp.Tag;
			stp.Children[0].Visibility = Visibility.Collapsed;
			stp.Children[1].Visibility = Visibility.Visible;
			this.Dispatcher.BeginInvoke(delegate()
			{
				ToggleCategoryVisible(false, tagValue);
			});
		}
		private void CategoryShow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			FrameworkElement ctl = sender as FrameworkElement;
			Panel stp = ctl.Parent as Panel;
			string tagValue = (string)stp.Tag;
			stp.Children[0].Visibility = Visibility.Visible;
			stp.Children[1].Visibility = Visibility.Collapsed;
			this.Dispatcher.BeginInvoke(delegate()
			{
				ToggleCategoryVisible(true, tagValue);
			});
		}
		#endregion
	}
	#endregion
}
