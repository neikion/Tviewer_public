using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tviewer.Converter
{
    internal class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool flag && flag)
            {
                return Visibility.Visible;
            }
            if (parameter is bool isCollapsed && isCollapsed)
                return Visibility.Collapsed;
            return Visibility.Hidden;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visible && visible == Visibility.Visible;
        }
    }
}
