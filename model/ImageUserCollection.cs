using System.Collections.Generic;
using Tviewer.controller;

namespace Tviewer.model
{
    public class ImageUserCollection : NotifyPropertyChangedBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public long CollectionID=-1;

        private List<long> _contentIDList;
        public List<long> ContentIDList { get { return _contentIDList; } set { _contentIDList = value;OnPropertyChanged(); } }
        public ImageUserCollection()
        {
            _name = string.Empty;
            _contentIDList = new List<long>();
        }
    }
}
