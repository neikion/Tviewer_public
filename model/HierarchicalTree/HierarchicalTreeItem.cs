using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPF_Practice.Interfaces.HierarchicalTree;

namespace WPF_Practice.model.HierarchicalTree
{
    public class HierarchicalTreeItem : INotifyPropertyChanged , IHierarchicalTreeItem
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private IHierarchicalTreeItem? _parent;
        public IHierarchicalTreeItem? Parent
        {
            get { return _parent; }
            set { _parent = value; OnPropertyChanged(); }
        }

        private ICommand? _command;
        public ICommand? Command
        {
            get => _command;
            set { _command = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IHierarchicalTreeItem> Children { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public HierarchicalTreeItem()
        {
            _name = string.Empty;
            Children= new ObservableCollection<IHierarchicalTreeItem>();

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            var hendler = PropertyChanged;
            if (hendler != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
