using System.Windows;
using System.Windows.Input;
using WPF_Practice.model;

namespace WPF_Practice.view
{
    /// <summary>
    /// UserSettingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserSettingWindow : Window
    {
        public UserSettingWindow()
        {
            InitializeComponent();
        }
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            WpfExtensions.CallMethod(DataContext, nameof(OnPreviewKeyDown), e);
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
