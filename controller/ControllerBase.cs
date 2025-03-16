using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_Practice.Interfaces
{
    public abstract class ControllerBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            var hendler = PropertyChanged;
            if (hendler != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public virtual void OnEnabled() { }
    }
}
