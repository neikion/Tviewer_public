using System;
using Tviewer.model;
using Tviewer.model.DB;
using Tviewer.model.HierarchicalTree;
using Tviewer.model.Setting;
using Tviewer.view.OptionControl;

namespace Tviewer.controller
{
    internal class ContentSettingWindowController : HostControllerBase
    {
        private HierarchicalTreeItem _items;
        public HierarchicalTreeItem Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged(); }
        }

        private CommandCarrier<HierarchicalTreeItem> _selectedItemCommand;
        public CommandCarrier<HierarchicalTreeItem> SelectedItemCommand
        {
            get { return _selectedItemCommand; }
            set { _selectedItemCommand = value; OnPropertyChanged(); }
        }

        private CommandCarrier _aceeptCommand;
        public CommandCarrier AceeptCommand
        {
            get => _aceeptCommand;
            set { _aceeptCommand = value; OnPropertyChanged(); }
        }

        private CommandCarrier _cancelCommand;
        public CommandCarrier CancelCommand
        {
            get => _cancelCommand;
            set { _cancelCommand = value; OnPropertyChanged(); }
        }

        private ConfigObject _localConfigObject;
        private Action? ViewClose;
        public bool Acceptable => _localConfigObject.OptionList.Count > 0;

        public bool Cancelable { get; set; }

        private ImageListContent EditContent= new ImageListContent();
        private ImageListContent originalContent=new ImageListContent();

        public ContentSettingWindowController()
        {
            _items = new HierarchicalTreeItem()
            {
                Children =
                {
                    new HierarchicalTreeItem()
                    {
                        Name = "Tag",
                        Selected = true,
                        Command=new CommandCarrier(() =>
                        {
                            MoveTempView<ContentSettingView>(ControllerStore.Get<ContentSettingViewController>().InitMetaDataSettingView(EditContent, this));
                        })
                    },
                    new HierarchicalTreeItem()
                    {
                        Name = "Artwork",
                        Command = new CommandCarrier(() =>
                        {
                            MoveTempView<ContentSettingView>(ControllerStore.Get<ContentSettingViewController>().InitArtworkSettingView(EditContent, this));
                        })
                    }
                }
            };
            _selectedItemCommand = new CommandCarrier<HierarchicalTreeItem>((o) =>
            {
                if (o == null) return;
                o.Command?.Execute(null);
            });
            _aceeptCommand = new CommandCarrier(() =>
            {
                originalContent.CopyFrom(EditContent,false);
                Config.AcceptConfig(_localConfigObject);
                using myDB db = new myDB();
                _=db.UpdateDBContent(originalContent);
                _localConfigObject = Config.GetConfigObject;
            });
            _cancelCommand = new CommandCarrier(() =>
            {
                ViewClose?.Invoke();
            });
        }

        public ContentSettingWindowController init(ImageListContent content, Action CloseView)
        {
            originalContent = content;
            EditContent = new ImageListContent(content,true);
            ViewClose = CloseView;
            return this;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _localConfigObject = Config.GetConfigObject;
        }
    }
}
