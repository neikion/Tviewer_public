using System.Windows;

namespace Tviewer.view
{
    public partial class UserSettingWindow : Window
    {
        public UserSettingWindow()
        {
            InitializeComponent();
        }

        private void OnAcceptBtnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult!=true &&!CancleBtn.IsEnabled)
            {
                e.Cancel = true;
            }
        }
    }
}
