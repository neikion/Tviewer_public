using System;
using System.Windows;
using System.Windows.Input;
using WPF_Practice.Interfaces;
using WPF_Practice.model;

namespace WPF_Practice.controller
{

    public class MainWindowControlloer : HostControllerBase,IFullScreenable
    {

        private object? _mainContent;
        public override object? MainContent
        {
            get=> _mainContent;
            set
            {
               _mainContent= value;
                OnPropertyChanged();
            }
        }




        private WindowState _windowState;
        public WindowState WinState { get { return _windowState; } set { _windowState = value; OnPropertyChanged(); } }

        private WindowStyle _windowStyle;
        public WindowStyle WinStyle { get { return _windowStyle; } set { _windowStyle = value; OnPropertyChanged(); } }

        private ResizeMode _resizeMode;
        public ResizeMode WinResizeMode { get { return _resizeMode; } set { _resizeMode = value; OnPropertyChanged(); } }


        private CommandCarrier? _closeBtn;
        public CommandCarrier? CloseBtn { get { return _closeBtn; } set { _closeBtn = value; }}


        private CommandCarrier? _maximizeBtn;
        public CommandCarrier? MaximizeBtn { get { return _maximizeBtn; } set { _maximizeBtn = value; } }


        private CommandCarrier? _minimizeBtn;
        public CommandCarrier? MinimizeBtn { get { return _minimizeBtn; } set { _minimizeBtn = value; } }


        public string GetProjectName
        {
            get
            {
                return (Application.Current.MainWindow.GetType().Assembly.GetName().Name ?? string.Empty);
            }
        }

        private void SetMinimizeWindow()
        {
            WinState = WindowState.Minimized;
        }
        

        public MainWindowControlloer()
        {
            CloseBtn = new CommandCarrier((o) => { Close(); });
            MaximizeBtn = new CommandCarrier((o) => {
                if (WinState == WindowState.Normal)
                {
                    WinState = WindowState.Maximized;
                }
                else
                {
                    WinState = WindowState.Normal;
                }
            });
            MinimizeBtn = new CommandCarrier((o) => { SetMinimizeWindow(); });
            _windowStyle = WindowStyle.SingleBorderWindow;
            _resizeMode = ResizeMode.CanResize;
        }

        public void OnPreviewKeyDown(KeyEventArgs e)
        {
            var realkey = WpfExtensions.RealKey(e);
            if (realkey == Key.Enter && WpfExtensions.CheckModifiersKey(ModifierKeys.Alt))
            {
                SetFullScreen(WinState==WindowState.Normal);
                e.Handled = true;
            }
        }
        public void OnKeyDown(KeyEventArgs e)
        {
            var realkey = WpfExtensions.RealKey(e);
            switch (realkey)
            {
                case Key.Escape:
                    Close();
                    break;
                default:
                    break;
            }
        }

        public void SetFullScreen(bool Active)
        {
            if (Active)
            {
                WinStyle = WindowStyle.None;
                WinResizeMode = ResizeMode.NoResize;
                WinState = WindowState.Maximized;
            }
            else
            {
                WinResizeMode = ResizeMode.CanResize;
                WinState = WindowState.Normal;
                WinStyle = WindowStyle.SingleBorderWindow;
            }
        }

        public void Show()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            Application.Current.Shutdown();
        }

        public void ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}
