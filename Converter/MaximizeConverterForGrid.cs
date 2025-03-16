using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPF_Practice.Converter
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
