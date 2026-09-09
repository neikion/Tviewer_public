using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Tviewer.controller;

namespace Tviewer.model
{
    public static class ViewStore
    {
        private static Dictionary<Type, UserControl> _store;

        static ViewStore()
        {
            _store = new Dictionary<Type, UserControl>();
        }

        public static UserControl CreateView<T>(params object?[]? argus) where T : UserControl
        {
            var _obj = Activator.CreateInstance(typeof(T), argus) as UserControl;
            if (_obj == null)
            {
                throw new ArgumentException($"{typeof(T)} is not convert to UserControl");
            }
            return _obj;
        }

        public static UserControl CreateView(Type T, params object?[]? argus)
        {
            if (!T.IsAssignableTo(typeof(UserControl))) throw new ArgumentException($"{T} is not convert to UserControl");
            var _obj = Activator.CreateInstance(T, argus) as UserControl;
            if (_obj == null)
            {
                throw new ArgumentException($"{T} is not convert to UserControl");
            }
            return _obj;
        }

        public static UserControl GetView<T>(params object?[]? argus) where T: UserControl
        {
            if (_store.TryGetValue(typeof(T), out UserControl? result))
            {
                return result;
            }
            result=CreateView<T>(argus);
            _store.Add(typeof(T), result);
            return result;
        }

        public static UserControl GetView(Type T, params object?[]? argus)
        {
            if (!T.IsAssignableTo(typeof(UserControl))) throw new ArgumentException($"{T} is not convert to UserControl");
            if (_store.TryGetValue(T, out UserControl? result))
            {
                return result;
            }
            result = CreateView(T, argus);
            _store.Add(T, result);
            return result;
        }

        public static Window? FindGlobalWindow(ControllerBase controller)
        {
            foreach (Window item in App.Current.Windows)
            {
                if (item.DataContext is not null && item.DataContext.Equals(controller))
                {
                    return item;
                }
            }
            return null;
        }

    }
}
