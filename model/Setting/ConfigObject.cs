using System.Collections.Generic;

namespace Tviewer.model.Setting
{
    public struct ConfigObject
    {
        public List<ConfigOption> OptionList { get; set; }

        public bool SkiaLendering = false;
        public ConfigObject()
        {
            OptionList = new List<ConfigOption>();
        }
        public ConfigObject(ConfigObject value)
        {
            OptionList = new List<ConfigOption>(value.OptionList);
        }
    }
}
