using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Tviewer.controller;
using Tviewer.view.Modal;
using static Tviewer.controller.ModalWindowController;

namespace Tviewer.view
{
    public partial class ModalWindow : Window
    {
        private DoubleAnimation? closedAnimation;
        private DoubleAnimation? openAnimation;
        private AnimationClock? animClock;
        private double animProgress = 0;
        public ModalAnimation currnetAnimation => GetModalAnimation(this);


        public static bool? GetMyDialogResult(DependencyObject obj) { return (bool?)obj.GetValue(MyDialogResultProperty); }

        public static void SetMyDialogResult(DependencyObject obj, bool? value) { obj.SetValue(MyDialogResultProperty, value); }

        public static readonly DependencyProperty MyDialogResultProperty =
            DependencyProperty.RegisterAttached("MyDialogResult", typeof(bool?), typeof(ModalWindow), new PropertyMetadata(null, OnDialogResultChanged));


        public static ModalAnimation GetModalAnimation(DependencyObject obj) { return (ModalAnimation)obj.GetValue(ModalAnimationProperty); }

        public static void SetModalAnimation(DependencyObject obj, ModalAnimation value) { obj.SetValue(ModalAnimationProperty, value); }

        public static readonly DependencyProperty ModalAnimationProperty =
            DependencyProperty.RegisterAttached("ModalAnimation", typeof(ModalAnimation), typeof(ModalWindow), new PropertyMetadata(ModalAnimation.None));


        public ModalWindow() : this(null) { }

        public ModalWindow(object? Context)
        {
            DataContext = Context;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ModalWindowController controller && controller.OnLoadedAnimation)
            {
                if (currnetAnimation == ModalAnimation.None) return;
                NotificationManager.UpdateModalLocation += UpdateLocation;
                InitAnimation(currnetAnimation);
                BeginAnimation(TopProperty, openAnimation);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            NotificationManager.UpdateModalLocation -= UpdateLocation;
            if (DataContext is ModalWindowController controller)
            {
                controller.Close(false);
            }
        }

        private void InitAnimation(ModalAnimation animation)
        {
            switch (animation)
            {
                case ModalAnimation.None:
                    closedAnimation = null;
                    openAnimation = null;
                    break;

                case ModalAnimation.FloatingUp:
                    openAnimation = new DoubleAnimation()
                    {
                        From = Top + 60,
                        To = Top,
                        EasingFunction = new PowerEase() { Power = 2, EasingMode = EasingMode.EaseOut },
                        Duration = new Duration(TimeSpan.FromMilliseconds(300))
                    };
                    openAnimation.Completed += complatedOpenAnimation;

                    closedAnimation = new DoubleAnimation()
                    {
                        From = openAnimation.To,
                        To = openAnimation.To - 60,
                        EasingFunction = new PowerEase() { Power = 2, EasingMode = EasingMode.EaseOut },
                        Duration = new Duration(TimeSpan.FromMilliseconds(300))
                    };
                    closedAnimation.Completed += complatedCloseAnimation;
                    break;
            }
        }

        private void ShowClosedAnimation()
        {
            closedAnimation = new DoubleAnimation()
            {
                From = Top - (60 * animProgress),
                To = Top - 60,
                EasingFunction = new PowerEase() { Power = 2, EasingMode = EasingMode.EaseOut },
                Duration = new Duration(TimeSpan.FromMilliseconds(300 * (1 - animProgress))),
            };
            closedAnimation.Completed += complatedCloseAnimation;

            if (animClock == null)
            {
                animClock = closedAnimation.CreateClock();
                ApplyAnimationClock(TopProperty, animClock);
                animClock.Controller.Begin();
            }
            else
            {
                closedAnimation.ApplyAnimationClock(TopProperty, animClock);
                animClock.Controller.Resume();
            }
        }

        private async void complatedOpenAnimation(object? sender, EventArgs e)
        {
            openAnimation?.Completed -= complatedOpenAnimation;
            if (animProgress == 0) await Task.Delay(2000);
            if (GetMyDialogResult(this) == null) { SetMyDialogResult(this, true); }
            else { ShowClosedAnimation(); }
        }

        private void complatedCloseAnimation(object? sender, EventArgs e)
        {
            closedAnimation?.Completed -= complatedCloseAnimation;
            Close();
        }

        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ModalWindow window)
            {
                switch (window.currnetAnimation)
                {
                    case ModalAnimation.FloatingUp:
                        window.ShowClosedAnimation();
                        return;
                }
                window.Close();
            }
        }

        private void UpdateLocation(ModalEventArgs args)
        {
            if (openAnimation == null || closedAnimation == null) return;
            
            if (animClock != null)
            {
                animClock.Controller.Pause();
                if (animClock.CurrentProgress != null)
                {
                    animProgress = animClock.CurrentProgress.Value;
                }
            }

            switch (args)
            {
                case ModalEventArgs.Add:
                    BeginAnimation(TopProperty, null);
                    openAnimation.Completed -= complatedOpenAnimation;
                    openAnimation = new DoubleAnimation()
                    {
                        From = Top,
                        To = openAnimation.To - ActualHeight,
                        Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                        EasingFunction = new PowerEase() { Power = 4, EasingMode = EasingMode.EaseOut },
                    };
                    openAnimation.Completed += complatedOpenAnimation;
                    BeginAnimation(TopProperty, openAnimation);
                    break;

                case ModalEventArgs.Delete:
                    BeginAnimation(TopProperty, null);
                    openAnimation.Completed -= complatedOpenAnimation;
                    openAnimation = new DoubleAnimation()
                    {
                        From = Top,
                        To = openAnimation.To + ActualHeight,
                        Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                        EasingFunction = new PowerEase() { Power = 4, EasingMode = EasingMode.EaseOut },
                        BeginTime = openAnimation.BeginTime
                    };
                    openAnimation.Completed += complatedOpenAnimation;
                    BeginAnimation(TopProperty, openAnimation);
                    break;
            }
        }

        private void OnMouseDown(object? sender, EventArgs e)
        {
            if (currnetAnimation == ModalAnimation.FloatingUp) Close();
        }
    }
}
