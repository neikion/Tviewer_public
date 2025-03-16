using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Practice.view.SearchBar
{
    /// <summary>
    /// SearchBar.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            InitializeComponent();
        }
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Hint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(nameof(Hint), typeof(string), typeof(SearchBar), new PropertyMetadata(string.Empty));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BarText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SearchBar), new PropertyMetadata(string.Empty));



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(SearchBar), new PropertyMetadata(false));



        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(SearchBar), new PropertyMetadata(string.Empty));


        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(SearchBar), new PropertyMetadata(null));

    }
}
