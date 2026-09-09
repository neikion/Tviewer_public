using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tviewer.view.TitleBar
{
    public partial class TitleBarControl : UserControl
    {

        public static readonly DependencyProperty OnCloseProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));
        public static readonly DependencyProperty OnMaximizeCommandProperty =
            DependencyProperty.Register(nameof(MaximizeCommand), typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));
        public static readonly DependencyProperty OnMinimizeCommandProperty =
            DependencyProperty.Register(nameof(MinimizeCommand), typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));
        public static readonly DependencyProperty MenuCommandProperty =
    DependencyProperty.Register(nameof(MenuCommand), typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));
        public static readonly DependencyProperty TitleProperty =
    DependencyProperty.Register(nameof(Title), typeof(string), typeof(TitleBarControl), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty DoubleClickProperty =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));
        public static readonly DependencyProperty LeftClickCommandProperty =
            DependencyProperty.Register(nameof(LeftClickCommand), typeof(ICommand), typeof(TitleBarControl), new PropertyMetadata(null));
        public static readonly DependencyProperty MenuButtonVisibilityProperty =
            DependencyProperty.Register(nameof(MenuButtonVisibility), typeof(Visibility), typeof(TitleBarControl), new PropertyMetadata(Visibility.Visible));


        public event RoutedEventHandler? OnCloseClick;
        public event RoutedEventHandler? OnMaxmizeClick;
        public event RoutedEventHandler? OnMinimizeClick;
        public event RoutedEventHandler? OnMenuClick;

        public TitleBarControl()
        {
            InitializeComponent();
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(OnCloseProperty); }
            set { SetValue(OnCloseProperty, value); }
        }

        public ICommand MaximizeCommand
        {
            get { return (ICommand)GetValue(OnMaximizeCommandProperty); }
            set { SetValue(OnMaximizeCommandProperty, value); }
        }

        public ICommand MinimizeCommand
        {
            get { return (ICommand)GetValue(OnMinimizeCommandProperty); }
            set { SetValue(OnMinimizeCommandProperty, value); }
        }
        public ICommand MenuCommand
        {
            get { return (ICommand)GetValue(MenuCommandProperty); }
            set { SetValue(MenuCommandProperty, value); }
        }
        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickProperty); }
            set { SetValue(DoubleClickProperty, value); }
        }
        public ICommand LeftClickCommand
        {
            get { return (ICommand)GetValue(LeftClickCommandProperty); }
            set { SetValue(LeftClickCommandProperty, value); }
        }

        public Visibility MenuButtonVisibility
        {
            get { return (Visibility)GetValue(MenuButtonVisibilityProperty); }
            set { SetValue(MenuButtonVisibilityProperty, value); }
        }


        private void OnMaximizeBtnClick(object sender, RoutedEventArgs e)
        {
            OnMaxmizeClick?.Invoke(sender, e);
        }

        private void OnMnimizeBtnClick(object sender, RoutedEventArgs e)
        {
            OnMinimizeClick?.Invoke(sender, e);
        }

        private void OnCloseBtnClick(object sender, RoutedEventArgs e)
        {
            OnCloseClick?.Invoke(sender, e);
        }

        private void OnMenuBtnClick(object sender, RoutedEventArgs e)
        {
            OnMenuClick?.Invoke(sender, e);
        }

        private void StopEventRouting(object sender, MouseButtonEventArgs e)
        {
            //더블 클릭 시 화면이 커지는 것을 방지
            e.Handled = true;
        }
    }
}
