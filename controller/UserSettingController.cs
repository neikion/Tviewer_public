using System;
using Tviewer.model;
using Tviewer.model.HierarchicalTree;
using Tviewer.model.Setting;
using Tviewer.view.OptionControl;

namespace Tviewer.controller
{
    public class UserSettingController : HostControllerBase
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

        private CommandCarrier cancelCommand;
        public CommandCarrier CancelCommand { get => cancelCommand; set { cancelCommand = value; OnPropertyChanged(); } }

        public bool Acceptable => _localConfigObject.OptionList.Count > 0;

        public bool Cancelable { get; set; }

        private SetWorkSpaceController _workController;
        private SetWorkSpaceController WorkspaceController
        {
            get
            {
                return _workController;
            }
            set
            {
                _workController = value;
            }
        }

        private CommandCarrier<ConfigObject> ApplyOption;

        private ConfigObject _localConfigObject;
        private Action? ViewClose;

        public UserSettingController()
        {
            _workController = ControllerStore.Get<SetWorkSpaceController>();
            ApplyOption = new CommandCarrier<ConfigObject>((config) =>
            {
                _localConfigObject = config;
                OnPropertyChanged(nameof(Acceptable));
            });
            _items = new HierarchicalTreeItem()
            {
                Children =
                {
                    new HierarchicalTreeItem()
                    {
                        Name = "Environment Setting",
                        Children =
                        {
                            new HierarchicalTreeItem()
                            {
                                Name= "Location",
                                Selected = true,
                                Command= new CommandCarrier(() =>
                                {
                                    Move<SetWorkSpace>(WorkspaceController.Init(_localConfigObject, ApplyOption));
                                })
                            }
                        }
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
                Config.AcceptConfig(_localConfigObject);
                _localConfigObject = Config.GetConfigObject;
                using WorkSpaceScanner scanner = new WorkSpaceScanner();
                foreach (var value in _localConfigObject.OptionList)
                {
                    scanner.InsertDBContent(scanner.Scan(value));
                }
                ControllerStore.Get<MainPageController>().UpdateToHome();
            });
            cancelCommand = new CommandCarrier(() =>
            {
                ViewClose?.Invoke();
            });
        }

        public UserSettingController Init(Action closeable)
        {
            ViewClose = closeable;
            return this;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _localConfigObject = Config.GetConfigObject;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ViewClose?.Invoke();
        }

    }
}
