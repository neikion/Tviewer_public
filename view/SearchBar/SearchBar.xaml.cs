using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tviewer.view.SearchBar
{
    public partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            InitializeComponent();
        }
        

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(nameof(Hint), typeof(string), typeof(SearchBar), new PropertyMetadata(string.Empty));
        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SearchBar), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(SearchBar), new PropertyMetadata(false));

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(SearchBar), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ButtonCommandProperty =
           DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(SearchBar), new PropertyMetadata(null));
        
        public static readonly DependencyProperty TextFocusProperty =
            DependencyProperty.Register(nameof(IsTextFocus), typeof(bool), typeof(SearchBar), new PropertyMetadata(false));

        public static readonly DependencyProperty IsButtonTabStopProperty =
            DependencyProperty.Register(nameof(IsButtonTabStop), typeof(bool), typeof(SearchBar), new PropertyMetadata(false));


        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public bool IsTextFocus
        {
            get { return (bool)GetValue(TextFocusProperty); }
            set { SetValue(TextFocusProperty, value); }
        }

        public bool IsButtonTabStop
        {
            get { return (bool)GetValue(IsButtonTabStopProperty); }
            set { SetValue(IsButtonTabStopProperty, value); }
        }

        public bool IsTextBoxTabStop { get => !IsReadOnly; }


        private void OnFoucus(object sender, RoutedEventArgs e)
        {
            IsTextFocus = true;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            IsTextFocus = false;
        }
    }
}
