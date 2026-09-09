using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;
using Tviewer.controller;
using Tviewer.Interfaces;

namespace Tviewer.model
{
    internal class AwakeInitializer
    {
        public static void Scan()
        {
            var d = Assembly.GetExecutingAssembly();
            List<Type> awakeTypes = new List<Type>();
            foreach (var type in d.GetTypes())
            {
                if (type.IsAssignableTo(typeof(IAwake)) && type.IsClass)
                {
                    awakeTypes.Add(type);
                }
            }

            awakeTypes.Sort((s, e) =>
            {
                return s.Name.CompareTo(e.Name);
            });

            foreach (var type in awakeTypes)
            {
                if (type.IsAssignableTo(typeof(UserControl)))
                {
                    var control = (IAwake)ViewStore.GetView(type);
                    control.Awake();
                }
                else if (type.IsAssignableTo(typeof(ControllerBase)))
                {
                    var controller=(IAwake)ControllerStore.Get(type);
                    controller.Awake();
                }
                else
                {
                    try
                    {
                        IAwake? obj = Activator.CreateInstance(type) as IAwake;
                        if (obj is null)
                        {
                            throw new NotSupportedException($"{type.FullName} is not a supported class type");
                        }
                        obj.Awake();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
    }
}
