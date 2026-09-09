using System.Collections.ObjectModel;

namespace Tviewer.Interfaces.HierarchicalTree
{
    public interface IHierarchicalTreeItem
    {
        IHierarchicalTreeItem? Parent { get; set; }
        ObservableCollection<IHierarchicalTreeItem> Children { get; }
        bool Selected { get; set; }

        bool IsExpanded { get; set; }
    }
}
