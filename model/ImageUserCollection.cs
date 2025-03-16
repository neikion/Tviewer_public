using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Practice.model
{
    public class ImageUserCollection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        public ObservableCollection<DBContent> Contents;
        public ImageUserCollection()
        {
            _name = string.Empty;
            Contents = new ObservableCollection<DBContent>();
        }
        public virtual void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            var hendler = PropertyChanged;
            if (hendler != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
