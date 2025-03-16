using System;
using System.Collections.Generic;
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

namespace WPF_Practice.TitleBar
{
    /// <summary>
    /// TitleBar.xaml에 대한 상호 작용 논리
    /// </summary>
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
