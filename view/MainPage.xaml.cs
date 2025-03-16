using System;
using System.Windows.Controls;
using WPF_Practice.model;

namespace WPF_Practice.view
{
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void ListViewItemInputDown(object sender, EventArgs e)
        {
            if(sender is ListViewItem target)
            {
                if (target == null || !(target.IsSelected && target.IsFocused)) return;
                WpfExtensions.CallCommand(DataContext, nameof(ListViewItemInputDown), new object[] { target.DataContext, e });
            }
        }
    }
}
