using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using Tviewer.Attribute;
using Tviewer.controller;
using Tviewer.Interfaces;
using Tviewer.model;
using Tviewer.view.Modal;

namespace Tviewer.view.OptionControl
{
    public class ContentSettingViewController : ControllerBase
    {
        private ObservableCollection<IContentSettingItem>? editcontent = new();
        public ObservableCollection<IContentSettingItem>? Content { get => editcontent; set { editcontent = value; OnPropertyChanged(); } }

        private CommandCarrier<IContentSettingItem> openEditWindow;
        public CommandCarrier<IContentSettingItem> OpenEditWindow { get => openEditWindow; set { openEditWindow = value; OnPropertyChanged(); } }

        INavigateHost host;
        IOwnerSetter? OwnerSetter;

        public ContentSettingViewController()
        {
            OpenEditWindow = new CommandCarrier<IContentSettingItem>((item) =>
            {
                ModalWindowController controller = new ModalWindowController();
                ModalWindow window = new ModalWindow(controller);
                OwnerSetter?.SetOwner(this);
                var c = controller.ShowInputModal("input value. split ;" ,true);
                controller.Move<SimpleInputModal>(c);
                window.ShowDialog();
            });
        }

        private void init(INavigateHost host, IOwnerSetter? setter)
        {
            Content?.Clear();
            this.host = host;
            OwnerSetter = setter;
        }

        public ContentSettingViewController InitMetaDataSettingView(ImageListContent content, INavigateHost host, IOwnerSetter? setter=null)
        {
            init(host, setter);
            if (Content is null) return this;
            var prop = typeof(ImageListContent).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            for (int i = 0; i < prop.Length; i++)
            {
                EditableImageOption? temp = prop[i].GetCustomAttribute<EditableImageOption>(true);
                if (temp is null || temp.option is PrintOption.Artwork) continue;
                object? result = prop[i].GetValue(content);
                switch (result)
                {
                    case null:
                        Content.Add(new ContentSettingListItem<object>(true) { Name = prop[i].Name, Value = string.Empty });
                        continue;
                    case IList<string> list:
                        Content.Add(new ContentSettingListItem<string>(false) { Name = prop[i].Name, OriginalValues = list });
                        continue;
                    case int:
                    case long:
                        Content.Add(new ContentSettingListItem<long>(true)
                        {
                            Name = prop[i].Name,
                            Value = result,
                            singleValueGetter = Getter<long>(prop[i], content),
                            singleValueSetter = Setter<long>(prop[i], content),
                            IsNumericOnly = true
                        });
                        continue;
                    case string:
                        Content.Add(new ContentSettingListItem<string>(true)
                        {
                            Name = prop[i].Name,
                            Value = result,
                            singleValueGetter = Getter<string>(prop[i], content),
                            singleValueSetter = Setter<string>(prop[i], content),
                            IsNumericOnly = false
                        });
                        break;
                }
            }
            return this;
        }

        private Func<T> Getter<T>(PropertyInfo info, ImageListContent closerTarget)
        {
            var cons = Expression.Constant(closerTarget, typeof(ImageListContent));
            var property = Expression.Property(cons, info);
            var lambda = Expression.Lambda<Func<T>>(property);
            return lambda.Compile();
        }

        private Action<T> Setter<T>(PropertyInfo info, ImageListContent closerTarget)
        {
            var param = Expression.Parameter(typeof(T));
            var cons = Expression.Constant(closerTarget, typeof(ImageListContent));
            var property = Expression.Property(cons, info);
            var assing = Expression.Assign(property, param);
            var lambda = Expression.Lambda<Action<T>>(assing, param);
            return lambda.Compile();
        }

        public ContentSettingViewController InitArtworkSettingView(ImageListContent content, INavigateHost host, IOwnerSetter? setter=null)
        {
            init(host, setter);
            //throw new NotImplementedException();
            return this;
        }
    }
}
