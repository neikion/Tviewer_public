using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Tviewer.Interfaces.HierarchicalTree;

namespace Tviewer.view.HierarchicalTree
{
    public partial class HierarchicalTree : UserControl
    {
        public ICommand SelectedItemChangedCommand
        {
            get { return (ICommand)GetValue(SelectedItemChangedCommandProperty); }
            set { SetValue(SelectedItemChangedCommandProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(HierarchicalTree), new PropertyMetadata(null));


        public ObservableCollection<IHierarchicalTreeItem> ItemSource
        {
            get { return (ObservableCollection<IHierarchicalTreeItem>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register(nameof(ItemSource), typeof(ObservableCollection<IHierarchicalTreeItem>), typeof(HierarchicalTree), new PropertyMetadata(null));


        public HierarchicalTree()
        {
            InitializeComponent();
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemChangedCommand?.Execute(e.NewValue);
        }

        private void LoadedTreeView(object sender, RoutedEventArgs e)
        {
            if(ItemSource.Count > 0 && ItemSource[0] is IHierarchicalTreeItem item && item.Parent is not null)
            {
                FindChildItem(item.Parent, out var pathlist);
                var item2 = GetTreeViewItem(Tree, pathlist);
                item2?.IsSelected = true;
                item2?.Focus();
            }
        }

        /// <summary>
        /// Get TreeViewItem Hierarchical Index in IHierarchicalTreeItem
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool FindChildItem(IHierarchicalTreeItem root, out List<int> path)
        {
            path = new List<int>();
            var target = root;
            int index = 0;
            while (target is not null && (target != root || index < target.Children.Count))
            {
                if (target.Selected)
                {
                    path.Add(index);
                    return true;
                }
                if (target.Children.Count > 0 && index < target.Children.Count)
                {
                    path.Add(index);
                    target = target.Children[index];
                    index = 0;
                    continue;
                }
                if (index >= target.Children.Count - 1)
                {
                    index = path[path.Count - 1] + 1;
                    path.RemoveAt(path.Count - 1);
                    target = target.Parent;
                    continue;
                }
                index++;
            }
            return false;
        }

        /// <summary>
        /// Get TreeViewItem in TreeView
        /// </summary>
        /// ref : https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-find-a-treeviewitem-in-a-treeview
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private TreeViewItem? GetTreeViewItem(ItemsControl? root, List<int> path)
        {
            for (int i = 0, count = path.Count; i < count; i++)
            {
                if (root is null) return null;
                if (root.DataContext is IHierarchicalTreeItem result && result.Selected)
                {
                    return root as TreeViewItem;
                }

                // Expand the current container
                if (root is TreeViewItem && !((TreeViewItem)root).IsExpanded)
                {
                    root.SetValue(TreeViewItem.IsExpandedProperty, true);
                }

                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate.  Note that in the
                // virtualizing case even if the item is marked
                // expanded we still need to do this step in order to
                // regenerate the visuals because they may have been virtualized away.
                root.ApplyTemplate();
                if (root.Template.FindName("ItemsHost", root) is ItemsPresenter itemsPresenter)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter,
                    // so walk the descendents and find the child.
                    itemsPresenter = FindVisualChild<ItemsPresenter>(root);
                    if (itemsPresenter == null)
                    {
                        root.UpdateLayout();
                        itemsPresenter = FindVisualChild<ItemsPresenter>(root);
                    }
                }

                VirtualizingStackPanel virtualizingPanel = (VirtualizingStackPanel)VisualTreeHelper.GetChild(itemsPresenter, 0);
                
                // Ensure that the generator for this panel has been created.
                _ = virtualizingPanel.Children;

                int targetIndex = path[i];
                TreeViewItem? temp = null;
                for (int i2 = 0; i2 <= targetIndex; i2++)
                {
                    if (virtualizingPanel != null)
                    {
                        // Bring the item into view so that the container will be generated.
                        virtualizingPanel.BringIndexIntoViewPublic(targetIndex);
                        temp = (TreeViewItem)root.ItemContainerGenerator.ContainerFromIndex(targetIndex);
                    }
                    else
                    {
                        // Bring the item into view to maintain the
                        // same behavior as with a virtualizing panel.
                        temp = (TreeViewItem)root.ItemContainerGenerator.ContainerFromIndex(targetIndex);
                        root.BringIntoView();
                    }
                }
                root = temp;
            }
            return root as TreeViewItem;
        }

        private TreeViewItem? GetTreeViewItem(ItemsControl container)
        {
            if (container == null) return null;
            if (container.DataContext is IHierarchicalTreeItem result && result.Selected)
            {
                return container as TreeViewItem;
            }

            if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
            {
                container.SetValue(TreeViewItem.IsExpandedProperty, true);
            }

            container.ApplyTemplate();
            if (container.Template.FindName("ItemsHost", container) is ItemsPresenter itemsPresenter)
            {
                itemsPresenter.ApplyTemplate();
            }
            else
            {
                itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                if (itemsPresenter == null)
                {
                    container.UpdateLayout();
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                }
            }

            VirtualizingStackPanel virtualizingPanel = (VirtualizingStackPanel)VisualTreeHelper.GetChild(itemsPresenter, 0);
            _ = virtualizingPanel.Children;
            for (int i = 0, count = container.Items.Count; i < count; i++)
            {
                TreeViewItem subContainer;
                if (virtualizingPanel != null)
                {
                    virtualizingPanel.BringIndexIntoViewPublic(i);
                    subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                }
                else
                {
                    subContainer = (TreeViewItem)container.ItemContainerGenerator.ContainerFromIndex(i);
                    subContainer.BringIntoView();
                }
                if (subContainer != null)
                {
                    TreeViewItem? resultContainer = GetTreeViewItem(subContainer);
                    if (resultContainer != null) return resultContainer;
                    subContainer.IsExpanded = false;
                }
            }
            return null;
        }

        /// <summary>
        /// Search for an element of a certain type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="visual">The parent element.</param>
        /// <returns></returns>
        private T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child != null)
                {
                    if (child is T result)
                    {
                        return result;
                    }

                    T descendent = FindVisualChild<T>(child);
                    if (descendent != null)
                    {
                        return descendent;
                    }
                }
            }
            return null;
        }
    }
}
