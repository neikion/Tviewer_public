using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Practice.Interfaces.HierarchicalTree;
using WPF_Practice.model.HierarchicalTree;

namespace WPF_Practice.view.HierarchicalTree
{
    /// <summary>
    /// HierarchicalTree.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HierarchicalTree : UserControl
    {



        public ICommand SelectedItemChangedCommand
        {
            get { return (ICommand)GetValue(SelectedItemChangedCommandProperty); }
            set { SetValue(SelectedItemChangedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItemChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(HierarchicalTree), new PropertyMetadata(null));



        public ObservableCollection<IHierarchicalTreeItem> ItemSource
        {
            get { return (ObservableCollection<IHierarchicalTreeItem>)GetValue(ItemSourceProperty); }
            set { 
                SetValue(ItemSourceProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register(nameof(ItemSource), typeof(ObservableCollection<IHierarchicalTreeItem>), typeof(HierarchicalTree), new PropertyMetadata(null));


        public HierarchicalTree()
        {
            InitializeComponent();
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemChangedCommand?.Execute(e);
        }
    }
}
