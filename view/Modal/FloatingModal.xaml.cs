using System.Windows.Controls;

namespace Tviewer.view.Modal
{
    /// <summary>
    /// FloatingModal.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FloatingModal : UserControl
    {
        public FloatingModal()
        {
            InitializeComponent();
        }

        public FloatingModal(string title, string text) : this()
        {
            titleTextBlock.Text = title;
            textBlock.Text = text;
        }

    }
}
