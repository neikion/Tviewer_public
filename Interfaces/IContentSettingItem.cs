using System.ComponentModel;

namespace Tviewer.Interfaces
{
    public interface IContentSettingItem : INotifyPropertyChanged, IEditableObject
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public bool IsNumericOnly { get; set; }
        public bool IsSingleValue { get; }
    }
}
