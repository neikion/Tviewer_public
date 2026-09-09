using System;
using System.Globalization;
using System.Windows.Data;
using Tviewer.model;
using Tviewer.model.EventArgs;

namespace Tviewer.Converter
{
    class CheckBoxModalToEventArgsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is bool isCheck && values[1] is ImageUserCollection collection && values[2] is ImageListContent content)
            {
                return new CheckBoxModalEventArgs(isCheck, collection, content);
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
