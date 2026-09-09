using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tviewer.Converter
{
    public class MaximizeConverterForGrid : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = (Visibility)value;
            if (target== Visibility.Collapsed || target==Visibility.Hidden)
            {
                return 0;
            }
            return parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
