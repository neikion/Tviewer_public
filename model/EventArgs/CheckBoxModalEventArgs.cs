namespace Tviewer.model.EventArgs
{
    public class CheckBoxModalEventArgs : System.EventArgs
    {
        public readonly bool isChecked;
        public readonly ImageUserCollection SetCollection;
        public readonly ImageListContent Content;
        public CheckBoxModalEventArgs(bool isChecked, ImageUserCollection setCollection, ImageListContent content)
        {
            this.isChecked = isChecked;
            SetCollection = setCollection;
            Content = content;
        }
    }
}
