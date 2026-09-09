using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tviewer.controller
{
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
