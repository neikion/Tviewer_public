using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tviewer.Converter
{
    public class MaximizeConveterVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((WindowStyle)value == WindowStyle.None)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
