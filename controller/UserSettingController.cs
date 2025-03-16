using System.Windows;
using System.Windows.Input;
using WPF_Practice.Interfaces;
using WPF_Practice.model;
using WPF_Practice.model.HierarchicalTree;
using WPF_Practice.view.OptionControl;

namespace WPF_Practice.controller
{
    public class UserSettingController : HostControllerBase
    {
        private HierarchicalTreeItem _items;
        public HierarchicalTreeItem Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged(); }
        }

        private CommandCarrier _selectedItemCommand;
        public CommandCarrier SelectedItemCommand
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

        private object? _mainContent;
        public override object? MainContent
        {
            get => _mainContent;
            set { _mainContent = value; OnPropertyChanged(); }
        }

        public bool Acceptable
        {
            get
            {
                return _localConfigObejct.WorkSpace.Count > 0;
            }
        }

        public bool Cancelable
        {
            get;set;
        }

        private SetWorkSpaceController _workController;
        private SetWorkSpaceController WorkspaceController
        {
            get
            {
                if (_workController == null)
                {
                    //Todo : 나중에 여러 workspace설정 가능하게 만들 것.
                    _workController = new SetWorkSpaceController(new CommandCarrier<string>(
                            (text) =>
                            {
                                if (text == null) return;
                                _localConfigObejct.WorkSpace.Add(text);
                                OnPropertyChanged(nameof(Acceptable));
                            }
                    ));
                }
                return _workController;
            }
            set
            {
                _workController = value;
            }
        }

        private ConfigObject _localConfigObejct;

        public UserSettingController()
        {
            SetOption(Config.ConfigObject);
            var header = new HierarchicalTreeItem();
            header.Name = "Environment Setting";
            header.Parent = _items;
            var option = new HierarchicalTreeItem();
            option.Name = "Location";
            option.Parent = header;
            option.Command = new CommandCarrier((o) =>
            {
                //Todo : 나중에 여러 workspace설정 가능하게 만들 것.
                if (_localConfigObejct.WorkSpace.Count > 0)
                {
                    WorkspaceController.Text = _localConfigObejct.WorkSpace[_localConfigObejct.WorkSpace.Count-1];
                }
                Move<SetWorkSpace>(WorkspaceController);
            });
            _items = new HierarchicalTreeItem();
            header.Children.Add(option);
            _items.Children.Add(header);
            _selectedItemCommand = new CommandCarrier((o) =>
            {
                if (o == null) return;
                var target = ((RoutedPropertyChangedEventArgs<object>)o).NewValue;
                ((HierarchicalTreeItem)target).Command?.Execute(null);
            });

            _aceeptCommand = new CommandCarrier((o) =>
            {
                if (_localConfigObejct.WorkSpace.Count > 0)
                {
                    Config.AcceptConfig(_localConfigObejct);
                }
                else {
                    App.OpenModal("must set workspace location");
                }
            });
        }

        public void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (WpfExtensions.RealKey(e))
            {
                case Key.D1:
                    HierarchicalTreeItem target = Items;
                    while (target.Children.Count != 0)
                    {
                        target = (HierarchicalTreeItem)target.Children[0];
                    }
                    HierarchicalTreeItem item = new HierarchicalTreeItem();
                    item.Name = "test object";
                    item.Parent = target;
                    target.Children.Add(item);
                    break;
            }
        }

        public void SetOption(ConfigObject value)
        {
            _localConfigObejct = value;
            //Todo : 나중에 여러 workspace설정 가능하게 만들 것.
            if (_localConfigObejct.WorkSpace.Count > 0)
            {
                WorkspaceController.Text = _localConfigObejct.WorkSpace[_localConfigObejct.WorkSpace.Count-1];
            }
            else
            {
                WorkspaceController.Text = string.Empty;
            }
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            SetOption(Config.ConfigObject);
        }

    }
}
