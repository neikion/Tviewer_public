using System;
using System.Collections.Generic;
using Tviewer.controller;
using Tviewer.Interfaces;

namespace Tviewer.view.OptionControl
{
    public class ContentSettingListItem<T> : NotifyPropertyChangedBase, IContentSettingItem
    {
        public class ContentData : NotifyPropertyChangedBase
        {
            public T? SingleValue;
            private IList<T>? values;
            public IList<T>? Values { get=>values; set { values = value; OnPropertyChanged(); } }
        }
        ContentData editingData=new();
        ContentData backupData=new();
        private bool isEditing = false;
        private IList<T> originalValues;
        public IList<T> OriginalValues
        {
            set { originalValues = value; editingData.Values = value as List<T>; OnPropertyChanged(nameof(editingData)); }
        }

        public bool IsSingleValue { get; private set; }
        public bool IsNumericOnly { get; set; } = false;
        private string name=string.Empty;
        public string Name { get=>name; set { name = value; OnPropertyChanged(); } }
        public Action<T>? singleValueSetter;
        public Func<T>? singleValueGetter;


        public object Value
        {
            get
            {
                if (IsSingleValue)
                {
                    return editingData.SingleValue;
                }
                return editingData.Values;
            }
            set
            {
                if (IsSingleValue)
                {
                    editingData.SingleValue = ConvertSetterValue(value);
                }
                else
                {
                    editingData.Values = value as IList<T>;
                }
                OnPropertyChanged();
            }
        }
        

        public ContentSettingListItem(bool single)
        {
            this.IsSingleValue = single;
            OnPropertyChanged(nameof(IsSingleValue));
        }

        public T ConvertSetterValue(object value)
        {
            Type target = typeof(T);
            if (value is T result)
            {
                return result;
            }
            if (value is string)
            {
                if (target.Equals(typeof(long)))
                {
                    if (long.TryParse((string)value, out long temp))
                    {
                        return (T)((object)temp);
                    }
                }
                else if (target.Equals(typeof(string)))
                {
                    return (T)value;
                }
            }
            return (T)Value;
        }

        public void BeginEdit()
        {
            if (!isEditing)
            {
                isEditing = true;
                backupData = editingData;
            }
        }

        public void CancelEdit()
        {
            if (isEditing)
            {
                isEditing = false;
                editingData = backupData;
            }
        }

        public void EndEdit()
        {
            if (isEditing)
            {
                isEditing = false;
                backupData = editingData;
                if (IsSingleValue)
                {
                    singleValueSetter?.Invoke(editingData.SingleValue);
                }
                else
                {
                    originalValues.Clear();
                    foreach(T item in editingData.Values!)
                    {
                        originalValues.Add(item);
                    }
                }
            }
        }
    }
}
