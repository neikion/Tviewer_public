using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Tviewer.model;

namespace Tviewer.view.Modal
{
    public partial class CheckBoxModal : Popup
    {

        public enum CustomPlacement
        {
            Custom,
            RightUp
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(CheckBoxModal), new PropertyMetadata(null));


        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectCommandProperty); }
            set { SetValue(ItemSelectCommandProperty, value); }
        }
        public static readonly DependencyProperty ItemSelectCommandProperty =
            DependencyProperty.Register(nameof(ItemSelectedCommand), typeof(ICommand), typeof(CheckBoxModal), new PropertyMetadata(null));


        public object ItemSelectedCommandParameter
        {
            get { return (object)GetValue(ItemSelectedCommandParameterProperty); }
            set { SetValue(ItemSelectedCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty ItemSelectedCommandParameterProperty =
            DependencyProperty.Register(nameof(ItemSelectedCommandParameter), typeof(object), typeof(CheckBoxModal), new PropertyMetadata(null));



        public CustomPlacement CustomPlacementMode
        {
            get { return (CustomPlacement)GetValue(CustomPlacementProperty); }
            set { SetValue(CustomPlacementProperty, value); }
        }

        public static readonly DependencyProperty CustomPlacementProperty =
            DependencyProperty.Register(nameof(CustomPlacementMode), typeof(CustomPlacement), typeof(CheckBoxModal), new PropertyMetadata(CustomPlacement.Custom, OnCustomPlacementModeChange));

        private static void OnCustomPlacementModeChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is CheckBoxModal modal)
            {
                if (modal.CustomPlacementMode == CustomPlacement.RightUp)
                {
                    modal.CustomPopupPlacementCallback = modal.OnAlign;
                }
                else
                {
                    modal.CustomPopupPlacementCallback = null;
                }
            }
        }

        public CheckBoxModal() : base()
        {
            InitializeComponent();
        }

        private void OnOpened(object sender, EventArgs e)
        {
            list.SelectedIndex = 0;
            (list.ItemContainerGenerator.ContainerFromIndex(0) as UIElement)?.Focus();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var key = WpfExtensions.RealKey(e);
            if (key == Key.Tab || key == Key.Escape)
            {
                if (key == Key.Escape) e.Handled = true;
                IsOpen = false;
                PlacementTarget.Focus();
            }
        }

        private CustomPopupPlacement[] OnAlign(Size popupSize, Size targetSize, Point offset)
        {
            return new CustomPopupPlacement[] { 
                new CustomPopupPlacement(new Point(targetSize.Width - popupSize.Width, targetSize.Height), PopupPrimaryAxis.Horizontal) 
            };
        }
    }
}
