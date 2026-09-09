using System;
using System.Globalization;
using System.Windows.Data;

namespace Tviewer.Converter
{
    internal class DefaultMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is object[] array) return array;
            return new object[] { value };
        }
    }
}
