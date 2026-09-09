using Tviewer.model.SearchEngine;

namespace Tviewer.model
{
    public class SelectedTagObject
    {
        public SearchField filed;
        public string tag =string.Empty;
        public SelectedTagObject(SearchField filed, string tag)
        {
            this.filed = filed;
            this.tag = tag;
        }
    }
}
