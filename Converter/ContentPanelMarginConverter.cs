using System;
using System.Globalization;
using System.Windows.Data;

namespace Tviewer.Converter
{
    class ContentPanelMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double width) return Binding.DoNothing;
            var result = Math.Clamp(width * 0.0625, 0, 50);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
