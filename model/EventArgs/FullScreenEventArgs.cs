namespace Tviewer.model.EventArgs
{
    public class FullScreenEventArgs : System.EventArgs
    {
        public bool IsFullScreen;
        public FullScreenEventArgs(bool value)
        {
            IsFullScreen = value;
        }
    }
}
