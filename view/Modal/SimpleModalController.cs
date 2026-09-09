using System;
using System.Windows;
using Tviewer.controller;
using Tviewer.model;

namespace Tviewer.view.Modal
{
    public class SimpleModalController : ControllerBase
    {
        private string content=string.Empty;
        private string message=string.Empty;
        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get => message;
            set { message = value; OnPropertyChanged(); }
        }

        private CommandCarrier? acceptCommand=null;
        public CommandCarrier? AcceptCommand
        {
            get { return acceptCommand; }
            set { acceptCommand = value; OnPropertyChanged(); }
        }

        private string acceptText= "Accept";
        public string AcceptText { get { return acceptText; } set { acceptText = value; OnPropertyChanged(); } }
        
        private string cancleText="Cancel";
        public string CancleText { get { return cancleText; } set { cancleText = value; OnPropertyChanged(); } }
        
        private Visibility cancelVisibility;
        public Visibility CancelVisibility
        {
            get { return cancelVisibility; }
            set { cancelVisibility = value; OnPropertyChanged(); }
        }

        public SimpleModalController() { }

        public SimpleModalController(Action<bool> SetDialogResult)
        {
            AcceptCommand = new CommandCarrier(() =>
            {
                SetDialogResult(true);
            });
        }
    }
}
