using System;
using System.Windows;
using System.Windows.Controls;
using WPF_Practice.model;

namespace WPF_Practice.Interfaces
{
    public abstract class HostControllerBase : ControllerBase, INavigateHost
    {
        public abstract object? MainContent{get;set;}
        public void Move<T>(ControllerBase? controller = null, params object?[]? parameters) where T : UserControl
        {
            MainContent = GetControl<T>(controller,parameters);
        }
        public void Move<T>(ref object? HostControl, ControllerBase? controller = null, params object?[]? parameters) where T: UserControl
        {
            HostControl = GetControl<T>(controller,parameters);
        }
        private UserControl GetControl<T>(ControllerBase? controller = null, params object?[]? parameters) where T : UserControl
        {
            UserControl control = ViewStore.GetView<T>(parameters);
            if (controller != null)
            {
                control.DataContext = controller;
            }
            RoutedEventHandler? method = null;
            method = (sender, e) => { control.Focus(); control.Loaded -= method; };
            control.Loaded += method;
            return control;
        }
    }
}
