using System.Windows;
using System.Windows.Controls;
using Tviewer.Interfaces;

namespace Tviewer.view.OptionControl
{
    internal class ContentSettingTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? SingleTemplate { get; set; }
        public DataTemplate? MultipleTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IContentSettingItem element)
            {
                if (!element.IsSingleValue) return MultipleTemplate;
                return SingleTemplate;
            }
            return null;
        }
    }
}
