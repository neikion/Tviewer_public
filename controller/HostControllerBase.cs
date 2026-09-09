using System;
using System.Windows;
using System.Windows.Controls;
using Tviewer.Interfaces;
using Tviewer.model;
using Tviewer.model.EventArgs;

namespace Tviewer.controller
{
    /// <summary>
    /// <see cref="ControllerBase"/> that owns a <see cref="Window"/>
    /// </summary>
    public abstract class HostControllerBase : ControllerBase, INavigateHost, IOwnerSetter, IDisposable
    {
        private object? _mainContent;
        private bool disposedValue;

        public object? MainContent => _mainContent;

        public event EventHandler<FullScreenEventArgs>? FullScreenEvent;

        /// <summary>
        /// application shutdown or window close
        /// </summary>
        /// <param name="shutdown">if shutdown is false, this function not performs the shutdown</param>
        public void Close(bool shutdown)
        {
            if (MainContent is FrameworkElement content)
            {
                (content.DataContext as ControllerBase)?.OnDisable();
                content.DataContext = null;
            }
            Dispose();
            if (shutdown)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Switching scenes using store
        /// </summary>
        /// <typeparam name="ViewType"></typeparam>
        /// <param name="nextController"></param>
        /// <param name="viewParameters"></param>
        public void Move<ViewType>(ControllerBase? nextController, params object?[]? viewParameters) where ViewType : UserControl
        {
            if (MainContent is FrameworkElement content)
            {
                (content.DataContext as ControllerBase)?.OnDisable();
                content.DataContext = null;
            }
            MoveCore(GetControl<ViewType>(nextController, viewParameters), nextController);
        }

        /// <summary>
        ///  Switching created scenes. not saved to ViewStore
        /// </summary>
        /// <param name="view"></param>
        /// <param name="nextController"></param>
        protected void MoveTempView<ViewType>(ControllerBase? nextController = null, params object?[]? viewParameters) where ViewType : UserControl
        {
            if (MainContent is FrameworkElement content)
            {
                (content.DataContext as ControllerBase)?.OnDisable();
                content.DataContext = null;
            }
            MoveCore(GetControl(ViewStore.CreateView<ViewType>(viewParameters),nextController), nextController);
        }

        /// <summary>
        /// Switching scenes without current controller disable
        /// </summary>
        /// <typeparam name="ViewType"></typeparam>
        /// <typeparam name="NextController"></typeparam>
        public void MoveWithoutDisable<ViewType>(ControllerBase? controller) where ViewType : UserControl
            => MoveCore(GetControl<ViewType>(controller), controller);

        private void MoveCore(UserControl view, ControllerBase? controller)
        {
            _mainContent = view;
            OnPropertyChanged(nameof(MainContent));
            controller?.OnEnable();
        }

        private UserControl GetControl<T>(ControllerBase? controller = null, params object?[]? parameters) where T : UserControl
        {
            return GetControl(ViewStore.GetView<T>(parameters), controller);
        }

        private UserControl GetControl(UserControl view, ControllerBase? controller)
        {
            if (controller != null)
            {
                view.DataContext = controller;
            }
            RoutedEventHandler? method = null;
            method = (sender, e) => { view.Focus(); view.Loaded -= method; };
            view.Loaded += method;
            return view;
        }

        public void SetOwner(ControllerBase controller)
        {
            ViewStore.FindGlobalWindow(controller)?.Owner = ViewStore.FindGlobalWindow(this);
        }

        protected virtual void OnFullScreen(bool Active)
        {
            if (Active)
            {
                FullScreenEvent?.Invoke(this, new FullScreenEventArgs(true));
            }
            else
            {
                FullScreenEvent?.Invoke(this, new FullScreenEventArgs(false));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
                FullScreenEvent = null;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
