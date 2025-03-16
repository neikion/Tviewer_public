using System;
using System.Windows;
using System.Windows.Input;
using WPF_Practice.controller;
using WPF_Practice.model;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using WPF_Practice.Interfaces;

namespace WPF_Practice.view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,IWinDependency
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
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

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            WpfExtensions.CallMethod(DataContext, nameof(OnKeyDown), e);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            WpfExtensions.CallMethod(DataContext, nameof(OnPreviewKeyDown), e);
        }

        private void OnTitleDrag(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper=new WindowInteropHelper(this);
            //parameter description
            //wMsg : 161(WM_NCLBUTTONDOWN, 0x00A1)
            //wParam : 2(HTCAPTION)
            //lParam : 0 (none)
            SendMessage(helper.Handle, 161, 2, 0);

            //not need dll. but minimize animation not smooth
            /*if (controller.WinState == WindowState.Maximized)
            {
                SetWindowPositionBasedOnMouse(e);
            }
            DragMove();*/
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

        private void OnMaximize(object sender, MouseButtonEventArgs e)
        {
            WpfExtensions.CallCommand(DataContext, "MaximizeBtn", null);
        }
    }
}
