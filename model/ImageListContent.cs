using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Tviewer.Attribute;
using Tviewer.model.DB;

namespace Tviewer.model
{
    public class ImageListContent : DBContent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        
        private ImageSource? _imageSource;
        [EditableImageOption(PrintOption.Artwork)]
        public ImageSource? Source
        {
            get => _imageSource;
            set { _imageSource = value; OnPropertyChanged(); }
        }

        [EditableImageOption(PrintOption.MetaData)]
        public string Title
        {
            get => Name;
            set { Name = value; OnPropertyChanged(); }
        }

        public new string Path
        {
            get => base.Path;
            set { base.Path = value; OnPropertyChanged(); }
        }

        public new long ModifyTime
        {
            get => base.ModifyTime;
            set { base.ModifyTime = value; OnPropertyChanged(Date); }
        }

        public string Date
        {
            get => new DateTime(base.ModifyTime).ToString("yyyy-mm-dd HH:mm");
        }

        [EditableImageOption(PrintOption.MetaData)]
        public new long Rating
        {
            get => base.Rating;
            set { base.Rating = value; OnPropertyChanged(); }
        }

        [EditableImageOption(PrintOption.MetaData)]
        public new List<string> Artist
        {
            get => base.Artist;
            set { base.Artist = value; OnPropertyChanged(); }
        }

        [EditableImageOption(PrintOption.MetaData)]
        public new List<string> Group
        {
            get => base.Group;
            set { base.Group = value; OnPropertyChanged(); }
        }

        [EditableImageOption(PrintOption.MetaData)]
        public new long SeriesOrder
        {
            get { return base.SeriesOrder; }
            set { base.SeriesOrder = value; OnPropertyChanged(); }
        }

        [EditableImageOption(PrintOption.MetaData)]
        public new List<string> Series
        {
            get => base.Series;
            set { base.Series = value; OnPropertyChanged(); }
        }

        [EditableImageOption(PrintOption.MetaData)]
        public new List<string> Collection
        {
            get => base.Collection;
            set { base.Collection = value; OnPropertyChanged(); }
        }

        [EditableImageOption(PrintOption.MetaData)]
        public List<string> Tags
        {
            get => Other;
            set { Other = value; OnPropertyChanged(); }
        }

        public CommandCarrier<ImageListContent>? _onRightClick;
        public CommandCarrier<ImageListContent>? OnRightClick { get => _onRightClick; set { _onRightClick = value; OnPropertyChanged(); } }


        public ImageListContent() : base(string.Empty) { }

        public ImageListContent(DBContent content) : this(content,true) { }

        public ImageListContent(DBContent content, bool deepCopy) : base(content, deepCopy) { }

        public override void CopyFrom(DBContent value, bool deepCopy)
        {
            if(value is ImageListContent content)
            {
                Source = content.Source;
                OnRightClick = content.OnRightClick;
            }
            ID = value.ID;
            Title = value.Name;
            Path = value.Path;
            CreateTime = value.CreateTime;
            ModifyTime = value.ModifyTime;
            Rating = value.Rating;
            SeriesOrder = value.SeriesOrder;
            if (deepCopy)
            {
                Artist = new List<string>(value.Artist);
                Group = new List<string>(value.Group);
                Tags = new List<string>(value.Other);
                Collection = new List<string>(value.Collection);
                Series = new List<string>(value.Series);
            }
            else
            {
                Artist = value.Artist;
                Group = value.Group;
                Tags = value.Other;
                Collection = value.Collection;
                Series = value.Series;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }

}
