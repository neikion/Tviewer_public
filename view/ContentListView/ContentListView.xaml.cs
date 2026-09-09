using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Tviewer.model;

namespace Tviewer.view.ContentListView
{
    public partial class ContentListView : UserControl
    {
        public ContentListView()
        {
            InitializeComponent();
            contentView.ItemContainerGenerator.StatusChanged += myStatusChanged;
        }

        ~ContentListView()
        {
            if (contentView != null)
            {
                BindingOperations.ClearAllBindings(contentView);
                contentView.ItemContainerGenerator.StatusChanged -= myStatusChanged;
            }
        }

        public static readonly DependencyProperty MyItemClick =
            DependencyProperty.Register(nameof(ItemClick), typeof(ICommand), typeof(ContentListView), new PropertyMetadata(null));

        public static readonly DependencyProperty MyContentList =
            DependencyProperty.Register(nameof(ContentList), typeof(IEnumerable), typeof(ContentListView), new PropertyMetadata(null));

        public static readonly DependencyProperty ScrollToBottomProperty =
            DependencyProperty.Register(nameof(ScrollToBottom), typeof(ICommand), typeof(ContentListView), new PropertyMetadata(null));

        /// <summary>
        /// it's Bind manually. Because wpf default behavior set to -1 when Loaded
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
           DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(ContentListView), new PropertyMetadata(0));

        public static readonly DependencyProperty TagClickCommandProperty =
            DependencyProperty.Register(nameof(TagClickCommand), typeof(ICommand), typeof(ContentListView), new PropertyMetadata(null));

        public static readonly DependencyProperty CollectionSourceProperty =
            DependencyProperty.Register(nameof(CollectionSource), typeof(IEnumerable), typeof(ContentListView), new PropertyMetadata(null));

        public static readonly DependencyProperty CollectionChangedCommandProperty =
            DependencyProperty.Register(nameof(CollectionChangedCommand), typeof(ICommand), typeof(ContentListView), new PropertyMetadata(null));

        public static readonly DependencyProperty OpenContentSettingCommandProperty =
            DependencyProperty.Register(nameof(OpenContentSettingCommand), typeof(ICommand), typeof(ContentListView), new PropertyMetadata(null));


        public ICommand ItemClick
        {
            get { return (ICommand)GetValue(MyItemClick); }
            set { SetValue(MyItemClick, value); }
        }

        public ICommand ScrollToBottom
        {
            get { return (ICommand)GetValue(ScrollToBottomProperty); }
            set { SetValue(ScrollToBottomProperty, value); }
        }

        public ICommand TagClickCommand
        {
            get { return (ICommand)GetValue(TagClickCommandProperty); }
            set { SetValue(TagClickCommandProperty, value); }
        }

        public IEnumerable ContentList
        {
            get { return (IEnumerable)GetValue(MyContentList); }
            set { SetValue(MyContentList, value); }
        }

        /// <summary>
        /// it's Bind manually. Because wpf default behavior set to -1 when Loaded
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public IEnumerable CollectionSource
        {
            get { return (IEnumerable)GetValue(CollectionSourceProperty); }
            set { SetValue(CollectionSourceProperty, value); }
        }

        public ICommand CollectionChangedCommand
        {
            get { return (ICommand)GetValue(CollectionChangedCommandProperty); }
            set { SetValue(CollectionChangedCommandProperty, value); }
        }

        public ICommand OpenContentSettingCommand
        {
            get { return (ICommand)GetValue(OpenContentSettingCommandProperty); }
            set { SetValue(OpenContentSettingCommandProperty, value); }
        }


        private bool listViewNeedUpdate = false;


        private void ListViewItemClick(object sender, EventArgs e)
        {
            if (e is KeyEventArgs  keyEvent)
            {
                switch(WpfExtensions.RealKey(keyEvent))
                {
                    case Key.Back:
                        return;
                    case Key.Enter:
                        if (WpfExtensions.CheckModifiersKey(ModifierKeys.Control))
                        {
                            OnItemClick(sender, OpenContentSettingCommand, true);
                        }
                        else
                        {
                            OnItemClick(sender, ItemClick, false);
                        }
                        return;
                }
            }
            if(e is MouseEventArgs mouseEvent && mouseEvent.LeftButton == MouseButtonState.Pressed)
            {
                OnItemClick(sender, ItemClick, false);
            }
        }

        private void OnItemClick(object sender, ICommand? command, bool passContentToCommand)
        {
            ListViewItem? target = sender is ListViewItem temp ? temp : (sender as FrameworkElement)?.TemplatedParent as ListViewItem;
            if (target != null && target.IsSelected && target.IsFocused)
            {
                SelectedIndex = contentView.SelectedIndex;
                if (passContentToCommand)
                {
                    command?.Execute(target.Content);
                }
                else
                {
                    command?.Execute(SelectedIndex);
                }
            }
        }

        private void ListViewLoaded(object sender, RoutedEventArgs e)
        {
            contentView.SelectedIndex = SelectedIndex;
            contentView.ScrollIntoView(contentView.SelectedItem);
            contentView.UpdateLayout();
            listViewNeedUpdate = true;
        }

        private void myStatusChanged(object? sender, EventArgs e)
        {
            if (sender is ItemContainerGenerator icg)
            {

                if (icg.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated && contentView.SelectedIndex > -1 && listViewNeedUpdate)
                {
                    (icg.ContainerFromIndex(contentView.SelectedIndex) as ListViewItem)?.Focus();
                    listViewNeedUpdate = false;
                }
            }
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.OriginalSource is ScrollViewer viewer)
            {
                //ScrollableHeight must not minus
                if (viewer.ScrollableHeight > 500 && viewer.VerticalOffset >= viewer.ScrollableHeight - 500)
                {
                    if (ScrollToBottom == null || !ScrollToBottom.CanExecute(null))
                        return;
                    ScrollToBottom.Execute(null);
                }
            }
        }
    }
}
