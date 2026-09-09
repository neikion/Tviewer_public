using System.Windows.Controls;
using Tviewer.model;
using Tviewer.controller;

namespace Tviewer.Interfaces
{
    public interface INavigateHost
    {
        /// <summary>
        /// Switching scenes provided by <see cref="ViewStore"/> and Using <see cref="ControllerStore"/> <br/>
        /// </summary>
        /// <typeparam name="ViewType">target scene type</typeparam>
        /// <param name="nextController">target Controller</param>
        /// <param name="viewParameters">used in constructor when view is first created</param>
        public void Move<ViewType>(ControllerBase? nextController, params object?[]? viewParameters) where ViewType : UserControl;

        public void MoveWithoutDisable<ViewType>(ControllerBase? controller) where ViewType : UserControl;

        public void Close(bool shutdown);
    }
}
