using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF_Practice.view.SearchBar
{
    public class BoolOrTextVisibleConVerter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)values[0] || !string.IsNullOrEmpty(values[1] as string))
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
