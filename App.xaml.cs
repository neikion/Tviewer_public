using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using WPF_Practice.controller;
using WPF_Practice.model;
using WPF_Practice.view;
using WPF_Practice.view.BaseModal;

namespace WPF_Practice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStart(object sender, StartupEventArgs e)
        {
            SetMagickNet();
            MainWindow window = new MainWindow();
            var controller = new MainWindowControlloer();
            window.DataContext = controller;
            window.Show();
            if (!Config.readUserConfig())
            {
                OpenModal(window, "First excute\nInitialization is required");
                Config.OpenUserConfigWindow(window,false);
            }
            List<string> result=new List<string>();
            WorkSpaceScanner scanner = new WorkSpaceScanner();
            scanner.GetImageDirectoryList(ref result,Config.ConfigObject.WorkSpace[0]);
            controller.Move<MainPage>(new MainPageController(controller));
        }
        private void SetMagickNet()
        {
            if (!Directory.Exists($"{Environment.CurrentDirectory}/temp"))
            {
                Directory.CreateDirectory($"{Environment.CurrentDirectory}/temp");
            }
            MagickNET.SetTempDirectory($"{Environment.CurrentDirectory}/temp");
        }

        public static void OpenModal(Window window,string text)
        {
            BaseModalWindow modal = new BaseModalWindow(text);
            modal.Owner = window;
            modal.CancleVisible = Visibility.Collapsed;
            modal.ShowDialog();
        }
        public static void OpenModal(string text)
        {
            BaseModalWindow modal = new BaseModalWindow(text);
            modal.CancleVisible = Visibility.Collapsed;
            modal.ShowDialog();
        }
    }
}
