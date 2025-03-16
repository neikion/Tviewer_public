using System.Windows.Input;
using WPF_Practice.Interfaces;
using WPF_Practice.model;

namespace WPF_Practice.view.OptionControl
{
    public class SetWorkSpaceController : ControllerBase
    {
        private CommandCarrier _searchBtn;
        public CommandCarrier SearchBtn
        {
            get => _searchBtn;
            set
            {
                _searchBtn = value;
                OnPropertyChanged();
            }
        }
        private string? _text;
        public string? Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
        private string? _buttonText;
        public string? ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        public ICommand? Execute { get; set; }

        public SetWorkSpaceController()
        {
            _searchBtn = new CommandCarrier((o) =>
            {
                var target = FileUtil.UsedWinform.openDirectoryBrowser();
                if (!string.IsNullOrEmpty(target)) Text = target;
                Execute?.Execute(Text);
            });
            ButtonText = "검색";
        }
        public SetWorkSpaceController(ICommand executeCommand) : this()
        {
            Execute = executeCommand;
        }
    }
}
