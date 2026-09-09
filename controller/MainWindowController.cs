using System.Windows;
using System.Windows.Input;
using Tviewer.model;
using Tviewer.model.DB;
using Tviewer.model.SearchEngine;
using Tviewer.model.Setting;
using Tviewer.view;

namespace Tviewer.controller
{

    public class MainWindowControlloer : HostControllerBase
    {
        public SlidingPanelController SlidingController { get { return ControllerStore.Get<SlidingPanelController>(); } }

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

        private CommandCarrier<KeyEventArgs> _keyDown;
        public CommandCarrier<KeyEventArgs> KeyDown { get { return _keyDown; } set { _keyDown = value; } }

        private CommandCarrier<KeyEventArgs> _previewKeyDown;
        public CommandCarrier<KeyEventArgs> PreviewKeyDown { get { return _previewKeyDown; } set { _previewKeyDown = value; } }

        private CommandCarrier _imageButtonDown;

        public CommandCarrier ImageButtonDown { get { return _imageButtonDown; } set { _imageButtonDown = value; } }

        private CommandCarrier MoveToHome;

        private bool _slideState=true;
        /// <summary>
        /// true is sliding panel open
        /// </summary>
        public bool SlideState { get { return _slideState; } set { _slideState = value; OnPropertyChanged(); } }

        private bool _IsInputText = false;
        public bool IsInputText { get => _IsInputText; set { _IsInputText = value; OnPropertyChanged(); } }
        public CommandCarrier<bool> ChangeInputText;

        public string GetProjectName
        {
            get
            {
                return (this.GetType().Assembly.GetName().Name ?? string.Empty);
            }
        }

        public MainWindowControlloer()
        {
            CloseBtn = new CommandCarrier(() =>
            {
                Close(true);
            });
            MaximizeBtn = new CommandCarrier(() => {
                if (WinState == WindowState.Normal)
                {
                    WinState = WindowState.Maximized;
                }
                else
                {
                    WinState = WindowState.Normal;
                }
            });
            MinimizeBtn = new CommandCarrier(() => { WinState = WindowState.Minimized; });
            _windowStyle = WindowStyle.SingleBorderWindow;
            _resizeMode = ResizeMode.CanResize;
            
            KeyDown = new CommandCarrier<KeyEventArgs>((e) => {
                if (e != null)
                {
                    var realkey = WpfExtensions.RealKey(e);
                    switch (realkey)
                    {
                        case Key.Escape:
                            Close(true);
                            break;
                        default:
                            break;
                    }
                }
            });
            PreviewKeyDown = new CommandCarrier<KeyEventArgs>((e) =>
            {
                if (e !=null && !IsInputText)
                {
                    var realkey = WpfExtensions.RealKey(e);
                    if (realkey == Key.Enter && WpfExtensions.CheckModifiersKey(ModifierKeys.Alt))
                    {
                        OnFullScreen(WinState == WindowState.Normal);
                        e.Handled = true;
                    }
                    if(WpfExtensions.CheckModifiersKey(ModifierKeys.Control | ModifierKeys.Shift) && realkey == Key.Delete)
                    {
                        if(App.OpenModal("Reset DB?",this,true))
                        {
                            using myDB db = new myDB();
                            db.ClearDBData();
                            init();
                        }
                    }
                    switch (realkey)
                    {
                        case Key.L:
                            ImageButtonDown.Execute(null);
                            break;
                        case Key.H:
                            MoveToHome?.Execute(null);
                            break;
                    }
                }
            });
            ImageButtonDown = new CommandCarrier(() =>
            {
                SlideState = !SlideState;
                SlidingController.SlideState= SlideState;
            });
            MoveToHome = new CommandCarrier(() =>
            {
                if (MainContent is MainPage) ControllerStore.Get<MainPageController>().checkUpdateNeeds();
                Move<MainPage>(ControllerStore.Get<MainPageController>());
            });
            ChangeInputText = new CommandCarrier<bool>((v) =>
            {
                IsInputText = v;
            });
            init();
        }

        private void init()
        {
            if (!Config.ReadUserConfig())
            {
                App.OpenModal("First excute\nInitialization is required");
                App.OpenUserConfigWindow(null, false);
            }
            SlidingController.init(this, ImageButtonDown, MoveToHome, this);
            MoveWithoutDisable<MainPage>(ControllerStore.Get<MainPageController>().Init(this,ChangeInputText));
        }

        protected override void OnFullScreen(bool Active)
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
            base.OnFullScreen(Active);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SearchEngine.Dispose();
        }
    }
}
