using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Tviewer.Converter
{
    public class BoolToCursorVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool flag)
            {
                if(!flag) return Cursors.None;
                return null;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
