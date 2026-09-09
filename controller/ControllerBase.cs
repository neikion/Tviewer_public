namespace Tviewer.controller
{
    public abstract class ControllerBase : NotifyPropertyChangedBase
    {
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
    }
}
