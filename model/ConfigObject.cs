using System.Collections.Generic;

namespace WPF_Practice.model
{
    public struct ConfigObject
    {
        public List<string> WorkSpace { get; set; }

        public ConfigObject()
        {
            WorkSpace = new List<string>();
        }
        public ConfigObject(ConfigObject value)
        {
            WorkSpace = new List<string>(value.WorkSpace);
        }
    }
}
