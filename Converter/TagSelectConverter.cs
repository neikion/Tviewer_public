using System;
using System.Globalization;
using System.Windows.Data;
using Tviewer.model;
using Tviewer.model.SearchEngine;

namespace Tviewer.Converter
{
    internal class TagSelectConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is SearchField filed && values[1] is string value)
            {
                return new SelectedTagObject(filed, value);
            }
            return new object();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
