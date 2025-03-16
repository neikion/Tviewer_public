using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Practice.controller;
using WPF_Practice.Interfaces;

namespace WPF_Practice.view
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext.GetType() == typeof(ImageViewController))
            {
                ((ImageViewController)DataContext).OnKeyDown(sender, e);
            }
            
        }
    }
}
