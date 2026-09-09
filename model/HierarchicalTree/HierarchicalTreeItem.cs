using System.Collections.ObjectModel;
using System.Windows.Input;
using Tviewer.controller;
using Tviewer.Interfaces.HierarchicalTree;

namespace Tviewer.model.HierarchicalTree
{
    public class HierarchicalTreeItem : NotifyPropertyChangedBase , IHierarchicalTreeItem
    {
        private string _name = string.Empty;
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

        public bool Selected { get; set; }
        public bool IsExpanded { get;set; }

        public HierarchicalTreeItem()
        {
            Children= new ObservableCollection<IHierarchicalTreeItem>();
            Children.CollectionChanged += OnChangedChildren;
        }

        private void OnChangedChildren(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (IHierarchicalTreeItem item in e.NewItems)
                {
                    item.Parent = this;
                    if(item.Selected || item.IsExpanded) IsExpanded = true;
                }
            }
            if(e.OldItems is not null)
            {
                foreach(IHierarchicalTreeItem oldItem in e.OldItems)
                {
                    oldItem.Parent = null;
                }
            }
        }
    }
}
