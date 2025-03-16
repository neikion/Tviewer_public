using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using WPF_Practice.Interfaces;

namespace WPF_Practice.model
{
    public class ScreenHelper : IWinformUsing
    {
        public static IEnumerable<ScreenHelper> AllScreens()
        {
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                yield return new ScreenHelper(screen);
            }
        }

        public static ScreenHelper GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            ScreenHelper wpfScreen = new ScreenHelper(screen);
            return wpfScreen;
        }

        private readonly System.Windows.Forms.Screen screen;

        public ScreenHelper(System.Windows.Forms.Screen screen)
        {
            this.screen = screen;
        }
        public double Width
        {
            get { return screen.Bounds.Width; }
        }
        public double Height
        {
            get { return screen.Bounds.Height; }
        }
        public Rectangle WorkingArea { get { return screen.WorkingArea; } }
    }
}
