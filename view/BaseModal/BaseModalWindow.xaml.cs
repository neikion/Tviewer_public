using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Practice.view.BaseModal
{
    /// <summary>
    /// BaseModalWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BaseModalWindow : Window
    {

        TextBlock? textBlock;

        public BaseModalWindow()
        {
            InitializeComponent();
            
        }

        public BaseModalWindow(string text) : this()
        {
            textBlock = new TextBlock();
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Text = text;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            MainContent = textBlock;
        }

        public string? ContentText
        {
            get
            {
                return textBlock?.Text;
            }
            set
            {
                if(textBlock != null) textBlock.Text = value;
            }
        }

        public object MainContent
        {
            get { return (object)GetValue(MainContentProperty); }
            set { SetValue(MainContentProperty, value); }
        }

        public static readonly DependencyProperty MainContentProperty =
            DependencyProperty.Register(nameof(MainContent), typeof(object), typeof(BaseModalWindow), new PropertyMetadata(null));



        public bool Acceptable
        {
            get { return (bool)GetValue(AcceptableProperty); }
            set { SetValue(AcceptableProperty, value); }
        }

        public static readonly DependencyProperty AcceptableProperty =
            DependencyProperty.Register(nameof(Acceptable), typeof(bool), typeof(BaseModalWindow), new PropertyMetadata(true));



        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AceeptCommandProperty); }
            set { SetValue(AceeptCommandProperty, value); }
        }

        public static readonly DependencyProperty AceeptCommandProperty =
            DependencyProperty.Register(nameof(AcceptCommand), typeof(ICommand), typeof(BaseModalWindow), new PropertyMetadata(null));



        public string AcceptText
        {
            get { return (string)GetValue(AcepptTextProperty); }
            set { SetValue(AcepptTextProperty, value); }
        }

        public static readonly DependencyProperty AcepptTextProperty =
            DependencyProperty.Register(nameof(AcceptText), typeof(string), typeof(BaseModalWindow), new PropertyMetadata("Accept"));



        public string CancleText
        {
            get { return (string)GetValue(CancleTextProperty); }
            set { SetValue(CancleTextProperty, value); }
        }

        public static readonly DependencyProperty CancleTextProperty =
            DependencyProperty.Register(nameof(CancleText), typeof(string), typeof(BaseModalWindow), new PropertyMetadata("Cancle"));




        public Visibility CancleVisible
        {
            get { return (Visibility)GetValue(CancleVisibleProperty); }
            set { SetValue(CancleVisibleProperty, value); }
        }

        public static readonly DependencyProperty CancleVisibleProperty =
            DependencyProperty.Register(nameof(CancleVisible), typeof(Visibility), typeof(BaseModalWindow), new PropertyMetadata(Visibility.Visible));




        private void OnAcceptBtnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
