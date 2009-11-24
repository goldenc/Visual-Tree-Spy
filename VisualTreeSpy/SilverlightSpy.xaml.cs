using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using ShaderEffectLibrary;

namespace VisualTreeSpy
{
    public partial class SilverlightSpy
    {
        private readonly ObservableCollection<VisualTreeItem> _items = new ObservableCollection<VisualTreeItem>();
        private UIElement _previousElement;
        private Dictionary<TreeViewItem, bool> _itemsBeingListened = new Dictionary<TreeViewItem, bool>();

        public SilverlightSpy()
        {
            TargetElement = Application.Current.RootVisual;
            InitializeComponent();

            OverlayOpacity = 0;

            ParentLayoutRoot = VisualTreeHelper.GetChild(Application.Current.RootVisual, 0) as Panel;
            Closing += SilverlightSpy_Closing;
        }

        void SilverlightSpy_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            treeView.SelectedItemChanged -= treeView_SelectedItemChanged;
            ClearEffect();
        }

        public DependencyObject TargetElement { get; set; }

        private void SetupRootSource()
        {
            _items.Clear();
            pgrid.SelectedObject = TargetElement;
            _items.Add(new VisualTreeItem
                           {
                               Tag = TargetElement,
                               Children = CreateEmptyCollection(TargetElement)
                           });
            treeView.ItemsSource = _items;
        }

        private TreeViewItem GetTreeViewItem(VisualTreeItem item)
        {
            return treeView.GetContainerFromItem(item);
        }

        private ObservableCollection<VisualTreeItem> CreateEmptyCollection(DependencyObject element)
        {
            var collection = new ObservableCollection<VisualTreeItem>();
            if (VisualTreeHelper.GetChildrenCount(element) > 0)
            {
                collection.Add(new VisualTreeItem());
            }
            return collection;
        }


        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = treeView.SelectedItem as VisualTreeItem;
            if (item != null)
            {
                var element = item.Tag as UIElement;

                ClearEffect();

                if (element != null)
                {
                    var effect = new InvertColorEffect();
                    element.Effect = effect;
                    _previousElement = element;
                }
                pgrid.SelectedObject = item.Tag;
            }
        }

        private void ClearEffect()
        {
            if (_previousElement != null)
            {
                _previousElement.Effect = null;
            }
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            var tvi = (TreeViewItem) sender;
            //var item = (VisualTreeItem)treeView.SelectedItem;
            var item = tvi.DataContext as VisualTreeItem;
            if (item != null)
            {
                var element = item.Tag as UIElement;
            
                if (!item.ChildrenSet)
                {
                    var childCount = VisualTreeHelper.GetChildrenCount(element);
                    var children = new ObservableCollection<VisualTreeItem>();
                    for (var i = 0; i < childCount; i++)
                    {
                        var child = VisualTreeHelper.GetChild(element, i);
                        children.Add(new VisualTreeItem { Tag = child, Children = CreateEmptyCollection(child) });
                    }
                    item.Children = children;
                    item.OnChildrenUpdated();
                }
                pgrid.SelectedObject = item.Tag;
            }
        }

        private void treeView_LayoutUpdated(object sender, EventArgs e)
        {
            if (_items.Count > 0)
            {
                AddExpandedHandlers(_items[0]);
            }
        }

        private void AddExpandedHandlers(VisualTreeItem item)
        {
            foreach (var child in item.Children)
            {
                AddExpandedHandlers(child);
            }

            var tvi = GetTreeViewItem(item);
            if (tvi != null && !_itemsBeingListened.ContainsKey(tvi))
            {
                _itemsBeingListened[tvi] = true;
                tvi.Expanded += TreeViewItem_Expanded;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetupRootSource();
        }

        private void btnFocusOnSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            if(treeView.SelectedItem != null)
            {
                TargetElement = ((VisualTreeItem) treeView.SelectedItem).Tag;
                SetupRootSource();
            }
        }

        private void btnClearEffect_Click(object sender, RoutedEventArgs e)
        {
            ClearEffect();
        }
    }
}