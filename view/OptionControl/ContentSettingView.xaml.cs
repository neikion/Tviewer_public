using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tviewer.model;

namespace Tviewer.view.OptionControl
{
    public partial class ContentSettingView : UserControl
    {
        [GeneratedRegex(@"^[0-9]+$")]
        private static partial Regex OnlyNumericRegex { get; }

        public ContentSettingView()
        {
            InitializeComponent();
        }
        public static bool GetIsNumericOnly(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsNumericOnlyProperty);
        }

        public static void SetIsNumericOnly(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericOnlyProperty, value);
        }

        public static readonly DependencyProperty IsNumericOnlyProperty =
            DependencyProperty.RegisterAttached("IsNumericOnly", typeof(bool), typeof(ContentSettingView), new PropertyMetadata(false, OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                if ((bool)e.NewValue)
                {
                    tb.PreviewTextInput += OnPreviewTextInput;
                    tb.PreviewKeyDown += OnKeyDown;
                }
                else
                {
                    tb.PreviewTextInput -= OnPreviewTextInput;
                    tb.PreviewKeyDown -= OnKeyDown;
                }
            }
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (WpfExtensions.RealKey(e).Equals(Key.Space))
            {
                e.Handled = true;
            }
        }



        private void OnGotFocusSelect(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                if (grid.SelectedCells.Count == 0) grid.SelectedCells.Add(grid.CurrentCell);
            }
        }

        private static void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!OnlyNumericRegex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void OnCellEditKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                switch (WpfExtensions.RealKey(e))
                {
                    case Key.Enter:
                        if (e.OriginalSource is DataGridCell cell && !cell.IsEditing)
                        {
                            grid.BeginEdit();
                        }
                        else
                        {
                            grid.CommitEdit();
                        }
                        e.Handled = true;
                        break;
                    case Key.Escape:
                        grid.CancelEdit();
                        e.Handled = true;
                        break;
                }
            }
        }

        private void OnLoadedCellFocus(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement item)
            {
                item.Focus();
            }
        }
    }
}
