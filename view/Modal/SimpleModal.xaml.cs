using System.Windows.Controls;

namespace Tviewer.view.Modal
{
    /// <summary>
    /// BaseModalControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SimpleModal : UserControl
    {
        public SimpleModal() { InitializeComponent(); }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AcceptButton.Focus();
        }
    }
}
