using System;
using System.Windows.Controls;

namespace WPF_Practice.Interfaces
{
    public interface INavigateHost
    {
        public void Move<T>(ControllerBase? controller=null,params object?[]? parameters) where T : UserControl;
    }
}
