using System;
using System.Collections.Generic;
using WPF_Practice.Interfaces;

namespace WPF_Practice.model
{
    public class ControllerStore
    {
        private static Dictionary<Type, ControllerBase> controllerStore;

        public static T Get<T>(params object?[]? parameters) where T : ControllerBase
        {
            T? result;
            if (controllerStore.ContainsKey(typeof(T)))
            {
                result = (T)controllerStore[typeof(T)];
            }
            else
            {
                result = Activator.CreateInstance(typeof(T), parameters) as T;
            }
            if (result == null)
            {
                throw new ArgumentException($"{typeof(T)} is not convert to UserControl");
            }
            result.OnEnabled();
            return result;
        }
        public static T GetNew<T>(params object?[]? parameters) where T : ControllerBase
        {
            T? result;
            if (controllerStore.ContainsKey(typeof(T)))
            {
                result = Activator.CreateInstance(typeof(T), controllerStore[typeof(T)]) as T;
            }
            else
            {
                result = Activator.CreateInstance(typeof(T), parameters) as T;
            }
            if (result == null)
            {
                throw new ArgumentException($"{typeof(T)} is not convert to UserControl");
            }
            result.OnEnabled();
            return result;
        }
        public static void Set<T>(T controller) where T : ControllerBase
        {
            if (controllerStore.ContainsKey(typeof(T)))
            {
                controllerStore[typeof(T)] = controller;
                return;
            }
            controllerStore.Add(typeof(T), controller);
        }

        static ControllerStore()
        {
            controllerStore = new Dictionary<Type, ControllerBase>();
        }
    }
}
