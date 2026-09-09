using System;
using System.Collections.Generic;
using Tviewer.controller;

namespace Tviewer.model
{
    public class ControllerStore
    {
        private static Dictionary<Type, ControllerBase> controllerStore;
        static ControllerStore()
        {
            controllerStore = new Dictionary<Type, ControllerBase>();
        }

        public static ControllerBase Get(Type T, params object?[]? parameters)
        {
            if(!T.IsAssignableTo(typeof(ControllerBase))) throw new ArgumentException($"{T} is not convert to ControllerBase");
            if (!controllerStore.TryGetValue(T, out ControllerBase? result))
            {
                result = Activator.CreateInstance(T, parameters) as ControllerBase;
                if (result == null)
                {
                    throw new ArgumentException($"{T} is not convert to UserControl");
                }
                controllerStore.Add(T, result);
            }
            return result;
        }

        public static T Get<T>(params object?[]? parameters) where T : ControllerBase
        {
            Type targetType = typeof(T);
            if (!controllerStore.TryGetValue(targetType, out ControllerBase? result))
            {
                result = Activator.CreateInstance(targetType, parameters) as ControllerBase;
                if (result == null)
                {
                    throw new ArgumentException($"{targetType} is not convert to UserControl");
                }
                controllerStore.Add(targetType, result);
            }
            return (T)result;
        }

        public static T GetNew<T>(params object?[]? parameters) where T : ControllerBase
        {
            T? result;
            result = Activator.CreateInstance(typeof(T), parameters) as T;
            if (result == null)
            {
                throw new ArgumentException($"{typeof(T)} is not convert to UserControl");
            }
            if (!controllerStore.ContainsKey(typeof(T)))
            {
                controllerStore.Add(typeof(T), result);
            }
            return result;
        }

        public static void Set<T>(T controller, bool disable=true) where T : ControllerBase
        {
            if(disable) controller.OnDisable();
            Type controllerType = controller.GetType();
            if (controllerStore.ContainsKey(controllerType))
            {
                controllerStore[controllerType] = controller;
                return;
            }
            controllerStore.Add(controllerType, controller);
        }

    }
}
