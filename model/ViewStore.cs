using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace WPF_Practice.model
{
    public static class ViewStore
    {
        private static Dictionary<Type, UserControl> _store;

        static ViewStore()
        {
            _store = new Dictionary<Type, UserControl>();

        }
        private static UserControl CreateUserControl<T>(params object?[]? argus) where T : UserControl
        {
            var _obj = Activator.CreateInstance(typeof(T), argus) as UserControl;
            if (_obj == null)
            {
                throw new ArgumentException($"{typeof(T)} is not convert to UserControl");
            }
            return _obj;
        }

        public static UserControl GetView<T>(params object?[]? argus) where T: UserControl
        {
            if (_store.ContainsKey(typeof(T)))
            {
                return _store[typeof(T)];
            }
            UserControl userControl=CreateUserControl<T>(argus);
            _store.Add(typeof(T), userControl);
            return userControl;
        }
        
    }
}
