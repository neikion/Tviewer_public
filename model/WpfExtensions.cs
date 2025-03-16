using System.Numerics;
using System.Windows.Input;

namespace WPF_Practice.model
{
    static class WpfExtensions
    {
        public static Key RealKey(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.System:
                    return e.SystemKey;

                case Key.ImeProcessed:
                    return e.ImeProcessedKey;

                case Key.DeadCharProcessed:
                    return e.DeadCharProcessedKey;

                default:
                    return e.Key;
            }
        }
        public static int ModifiersKeyCount(ModifierKeys keys)
        {
            return BitOperations.PopCount((uint)keys);
        }
        public static bool CheckModifiersKey(params ModifierKeys[] keys)
        {
            if (keys.Length != ModifiersKeyCount(Keyboard.Modifiers))
            {
                return false;
            }
            for(int i=0;i<keys.Length; i++)
            {
                if (!Keyboard.Modifiers.HasFlag(keys[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckModifiersKey(ModifierKeys keys)
        {
            return ModifiersKeyCount(keys) == ModifiersKeyCount(Keyboard.Modifiers) && Keyboard.Modifiers.HasFlag(keys);
        }
        public static void CheckFoucus()
        {
            var focusedElement = Keyboard.FocusedElement;
        }
        public static void CallMethod(object dataContext, string name, params object?[]? parameters)
        {
            dataContext.GetType().GetMethod(name)?.Invoke(dataContext, parameters);
        }
        public static void CallCommand(object dataContext, string name, params object?[]? parameters)
        {
            object? result = dataContext.GetType().GetProperty(name)?.GetGetMethod()?.Invoke(dataContext, null);
            if (result == null)
            {
                return;
            }
            ICommand command = (ICommand)result;
            if (command.CanExecute(parameters))
            {
                command.Execute(parameters);
            }
        }
    }
}
