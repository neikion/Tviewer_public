using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Tviewer.Interfaces;
using Tviewer.model;

namespace Tviewer.view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,IWinDependency
    {
        [LibraryImport("user32.dll",EntryPoint = "SendMessageW")]
        private static partial IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        public MainWindow()
        {
            InitializeComponent();
        }
        public double RealWidth
        {
            get { return Math.Max(Width, ActualWidth); }
        }
        public double RealHeight
        {
            get { return Math.Max(Height, ActualHeight); }
        }

        private void OnTitleDrag(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper=new WindowInteropHelper(this);
#if WINDOWS
            //parameter description
            //wMsg : 161(WM_NCLBUTTONDOWN, 0x00A1)
            //wParam : 2(HTCAPTION)
            //lParam : 0 (none)
            SendMessage(helper.Handle, 0x00A1, 2, 0);
#else
            if (WindowState == WindowState.Maximized)
            {
                ResizeBeforeDrag(e);
            }
            DragMove();
#endif
        }

        private void ResizeBeforeDrag(MouseButtonEventArgs e)
        {
            var sc = ScreenHelper.GetScreenFrom(this);
            var point = e.GetPosition(this);
            double per = (point.X) / (sc.WorkingArea.Width);
            double padding = 10;
            Left = PointToScreen(point).X - Math.Clamp((RestoreBounds.Width * per),TitleBar.ButtonImage.ActualWidth+ padding, RestoreBounds.Width-TitleBar.OperationGroup.ActualWidth- padding);
            Top = 10;
            WpfExtensions.CallMethod(DataContext, "SetFullScreen", false);
            Top = 10;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetFullScreenMargin((WindowState == WindowState.Maximized));
        }
        private void SetFullScreenMargin(bool Active)
        {
            if (Active)
            {
                var sc = ScreenHelper.GetScreenFrom(this);
                double _editWitdh = (RealWidth - sc.WorkingArea.Width) / 2;
                double _editHitdh = (RealHeight - sc.WorkingArea.Height) / 2;
                var _margin = Margin;
                if (WindowStyle == WindowStyle.None)
                {
                    _margin.Top = 0;
                    _margin.Bottom = 0;
                }
                else
                {
                    _margin.Top = _editHitdh;
                    _margin.Bottom = _editHitdh;
                }
                _margin.Left = _editWitdh;
                _margin.Right = _editWitdh;
                
                ContentGrid.Margin = _margin;
            }
            else
            {
                Thickness _margin = new Thickness();
                _margin.Top = 0;
                _margin.Left = 0;
                _margin.Right = 0;
                _margin.Bottom = 0;
                ContentGrid.Margin = _margin;
            }
        }
    }
}
