using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Tviewer.view.SlidingPanel
{
    public partial class SlidingPanel : UserControl
    {
        public bool SlidingState
        {
            get { return (bool)GetValue(MySlidingProperty); }
            set { SetValue(MySlidingProperty, value); }
        }
        public ICommand AddCollection
        {
            get { return (ICommand)GetValue(AddCollectionProperty); }
            set { SetValue(AddCollectionProperty, value); }
        }


        public static readonly DependencyProperty MySlidingProperty =
            DependencyProperty.Register("SlidingState", typeof(bool), typeof(SlidingPanel), new PropertyMetadata(true, SetSliding));

        private static void SetSliding(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SlidingPanel panel) panel.BeginSliding(); 
        }

        public static readonly DependencyProperty AddCollectionProperty =
            DependencyProperty.Register(nameof(AddCollection), typeof(ICommand), typeof(SlidingPanel), new PropertyMetadata(null));


        /// <summary>
        /// Init when loaded
        /// </summary>
        private Storyboard storyboard = new Storyboard();
        private DoubleAnimation ani = new DoubleAnimation();
        private double originalSize;

        public SlidingPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// this method is invoked when Loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Init(object? sender, RoutedEventArgs e)
        {
            originalSize = ActualWidth;
            ani.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            storyboard.Children.Add(ani);
            Storyboard.SetTarget(ani, this);
            Storyboard.SetTargetProperty(ani,new PropertyPath(WidthProperty));
        }
        public void BeginSliding()
        {
            if (SlidingState)
            {
                ani.From = 0;
                ani.To = originalSize;
            }
            else
            {
                ani.From = originalSize;
                ani.To = 0;
            }
            storyboard.Begin();
        }
    }
}
