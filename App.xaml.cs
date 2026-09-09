using ImageMagick;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Tviewer.controller;
using Tviewer.Interfaces;
using Tviewer.model;
using Tviewer.view;

namespace Tviewer
{
    public partial class App : Application
    {
        private void OnStart(object sender, StartupEventArgs e)
        {
            //Prevent textbox / passward box selection background from covering the foreground.
            //it must excute before init view
            AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);
            SetMagickNet();
            AwakeInitializer.Scan();
            MainWindow window = new MainWindow();
            window.DataContext = ControllerStore.Get<MainWindowControlloer>();
            window.Show();
        }

        private void SetMagickNet()
        {
            if (!Directory.Exists($"{Environment.CurrentDirectory}/temp"))
            {
                Directory.CreateDirectory($"{Environment.CurrentDirectory}/temp");
            }
            MagickNET.SetTempDirectory($"{Environment.CurrentDirectory}/temp");
            ImageMagick.OpenCL.SetCacheDirectory($"{Environment.CurrentDirectory}/temp");
            ImageMagick.OpenCL.IsEnabled = true;
            //ImageMagick.ResourceLimits.Throttle = 500;
            //ImageMagick.ResourceLimits.Thread = (ulong)(Environment.ProcessorCount/2);
        }

        public static bool OpenModal(string text, IOwnerSetter? host=null, bool cancel=false)
        {
            ModalWindowController controller = new ModalWindowController();
            controller.ShowBaseModal(text,cancel);
            ModalWindow modal = new ModalWindow(controller);
            host?.SetOwner(controller);
            modal.ShowDialog();
            return controller.DialogResult==true;
        }

        public static void OpenFloatingModal(string text, IOwnerSetter? setter=null)
        {
            ModalWindowController controller = new ModalWindowController();
            controller.ShowFloatingModal(text);
            ModalWindow modal = new ModalWindow(controller);
            setter?.SetOwner(controller);
            modal.Show();
        }

        public static string OpenSimpleInputModal(string message,out bool cancel, IOwnerSetter? host=null)
        {
            ModalWindowController controller = new ModalWindowController();
            var modalController=controller.ShowInputModal(message, true);
            ModalWindow modal=new ModalWindow(controller);
            host?.SetOwner(controller);
            modal.ShowDialog();
            if (controller.DialogResult == true)
            {
                cancel = false;
                return modalController.Content;
            }
            cancel = true;
            return string.Empty;
        }

        public static void OpenUserConfigWindow(IOwnerSetter? host, bool Cancelable = true)
        {
            UserSettingWindow window = new UserSettingWindow();
            UserSettingController? controller = ControllerStore.Get<UserSettingController>().Init(() => { window.DialogResult = true; });
            controller.Cancelable = Cancelable;
            window.DataContext = controller;
            if (host != null)
            {
                host?.SetOwner(controller);
                window.ShowInTaskbar = false;
            }
            else
            {
                window.ShowInTaskbar = true;
            }
            controller.OnEnable();
            window.ShowDialog();
        }

        public static void OpenContentSettingWindow(ImageListContent content, IOwnerSetter? host=null, bool Cancelable = true)
        {
            UserSettingWindow window = new UserSettingWindow();
            ContentSettingWindowController controller = ControllerStore.Get<ContentSettingWindowController>().init(content, () => { window.Close(); });
            controller.Cancelable = Cancelable;
            window.DataContext = controller;
            if (host != null)
            {
                host.SetOwner(controller);
                window.ShowInTaskbar = false;
            }
            else
            {
                window.ShowInTaskbar = true;
            }
            controller.OnEnable();
            window.ShowDialog();
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if ((button.ContextMenu is not null && button.ContextMenu.IsOpen)) return;
                VisualStateManager.GoToState(button, "MouseOver", true);
            }
        }
    }
}
