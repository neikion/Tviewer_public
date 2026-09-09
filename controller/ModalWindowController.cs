using System.Collections.Generic;
using System.Windows;
using Tviewer.model;
using Tviewer.view.Modal;

namespace Tviewer.controller
{
    public class ModalWindowController : HostControllerBase
    {
        public static class NotificationManager
        {
            public delegate void OnUpdateStatus(ModalEventArgs e);
            public static LinkedList<OnUpdateStatus> modalList = new LinkedList<OnUpdateStatus>();
            public static event OnUpdateStatus UpdateModalLocation
            {
                add
                {
                    if (modalList == null) return;
                    var node = modalList.First;
                    for (int i = 0; i < modalList.Count; i++)
                    {
                        node?.Value.Invoke(ModalEventArgs.Add);
                        node = node?.Next;
                    }
                    modalList.AddFirst(value);
                }
                remove
                {
                    LinkedListNode<OnUpdateStatus>? node = modalList.First;
                    bool findFlag = false;
                    while (node != null)
                    {
                        if (findFlag) node.Value.Invoke(ModalEventArgs.Delete);
                        if (node.Value == value)
                        {
                            var temp = node.Next;
                            findFlag = true;
                            modalList.Remove(node);
                            node = temp;
                            continue;
                        }
                        node = node.Next;
                    }
                }
            }
        }


        public bool OnLoadedAnimation = false;
        public bool OnCloseAnimation = false;


        private ModalAnimation playingAnimation = ModalAnimation.None;
        public ModalAnimation PlayingAnimation { get => playingAnimation; set { playingAnimation = value; OnPropertyChanged(); } }

        WindowStyle windowStyle = WindowStyle.None;
        public WindowStyle WindowStyle
        {
            get { return windowStyle; }
            set { windowStyle = value; OnPropertyChanged(); OnPropertyChanged(nameof(AllowsTransparency)); }
        }

        public bool AllowsTransparency
        {
            get
            {
                if (windowStyle == WindowStyle.None) return true;
                return false;
            }
        }

        bool showInTaskBar = false;
        public bool ShowInTaskBar
        {
            get { return showInTaskBar; }
            set { showInTaskBar = value; OnPropertyChanged(); }
        }

        bool? dialogResult = null;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                if (value is not null) OnDisable();
                dialogResult = value;
                OnPropertyChanged();
            }
        }

        private bool showActivated = false;
        public bool ShowActivated { get => showActivated; set { showActivated = value; OnPropertyChanged(); } }

        private bool topMost = false;
        public bool TopMost { get => topMost; set { topMost = value; OnPropertyChanged(); } }

        public ModalWindowController() { }

        public void ShowFloatingModal(string text)
        {
            ShowInTaskBar = false;
            this.WindowStyle = WindowStyle.None;
            OnLoadedAnimation = true;
            OnCloseAnimation = true;
            PlayingAnimation = ModalAnimation.FloatingUp;
            MoveTempView<FloatingModal>(this, "Alert", text);
        }

        public void ShowBaseModal(string text, bool Cancel = false)
        {
            ShowInTaskBar = true;
            this.WindowStyle = WindowStyle.ToolWindow;
            ShowActivated = true;
            var controller = new SimpleModalController()
            {
                AcceptCommand = new CommandCarrier(() =>
                {
                    DialogResult = true;
                }),
                Content = text
            };
            if (!Cancel) { controller.CancelVisibility = Visibility.Collapsed; }
            else { controller.CancelVisibility = Visibility.Visible; }
            PlayingAnimation = ModalAnimation.None;
            MoveTempView<SimpleModal>(controller);
        }

        public SimpleModalController ShowInputModal(string Message, bool Cancel=false)
        {
            ShowInTaskBar = true;
            showActivated = true;
            WindowStyle = WindowStyle.ToolWindow;
            var controller = new SimpleModalController()
            {
                AcceptCommand = new CommandCarrier(() =>
                {
                    DialogResult = true;
                }),
                Message = Message
            };
            if (!Cancel) { controller.CancelVisibility = Visibility.Collapsed; }
            else { controller.CancelVisibility = Visibility.Visible; }
            PlayingAnimation = ModalAnimation.None;
            MoveTempView<SimpleInputModal>(controller);
            return controller;
        }
    }
}
