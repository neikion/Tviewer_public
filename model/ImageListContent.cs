using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPF_Practice.model
{
    public class ImageListContent
    {
        public ImageSource Source { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string ModifyTime { get; set; }
        public ObservableCollection<string> Artist { get; set; }
        public ObservableCollection<string> Group { get; set; }
        public ObservableCollection<string> Tags { get; set; }
        
        public ImageListContent()
        {
            Source = null;
            Title = string.Empty;
            Path = string.Empty;
            Artist = new ObservableCollection<string>();
            Group = new ObservableCollection<string>();
            Tags = new ObservableCollection<string>();
        }
        public ImageListContent(DBContent content)
        {
            Title = content.Name;
            Path = content.Path;
            Artist = new ObservableCollection<string>(content.Artist);
            Group = new ObservableCollection<string>(content.Artist);
            Tags = new ObservableCollection<string>(content.Other);
            ModifyTime = new DateTime(content.ModifyTime).ToString("yyyy-mm-dd HH:mm");
        }
    }

}
