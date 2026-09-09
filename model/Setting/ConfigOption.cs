namespace Tviewer.model.Setting
{
    public struct ConfigOption
    {
        public ConfigOption()
        {
            ID = -1;
            WorkSpace=string.Empty;
        }
        public long ID;
        public string WorkSpace;
        public long SaveTime;
    }
}
