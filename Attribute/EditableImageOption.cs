using System;
using Tviewer.model;
using Tviewer.model.DB;
using Tviewer.view.OptionControl;

namespace Tviewer.Attribute
{
    public enum PrintOption
    {
        MetaData,
        Artwork,
    }

    /// <summary>
    /// Attribute used in <see cref="ContentSettingViewController"/> for selectively printing the data from <see cref="ImageListContent"/>, <see cref="DBContent"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =true, Inherited = true)]
    internal class EditableImageOption : System.Attribute 
    {
        public PrintOption option;
        public EditableImageOption(PrintOption option)
        {
            this.option = option;
        }
    }
}
