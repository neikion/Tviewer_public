using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_Practice.Converter
{
    public class MaximaizeConverterForGlassFrame : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((WindowStyle)value == WindowStyle.None)
            {
                return parameter;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
