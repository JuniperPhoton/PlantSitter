using JP.Utils.Framework;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace PlantSitter.Common
{
    public abstract class BasePage : Page
    {
        public event EventHandler<KeyEventArgs> GlobalPageKeyDown;

        public BasePage()
        {
            this.Loaded += BasePage_Loaded;
            SetUpPageAnimation();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is INavigable)
            {
                var page = this.DataContext as INavigable;
                page.OnLoaded();
            }
        }
        protected virtual void SetUpPageAnimation()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new DrillInNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;

            base.OnNavigatedTo(e);
            if (this.DataContext is INavigable)
            {
                var page = this.DataContext as INavigable;
                page.Activate(e.Parameter);
            }

            TitleBarHelper.SetupThemeTitleBar();

            //resolve global keydown
            if (GlobalPageKeyDown != null)
            {
                Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            }
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (this.DataContext is INavigable)
            {
                var page = this.DataContext as INavigable;
                page.Deactivate(e);
            }

            //resolve global keydown
            if (GlobalPageKeyDown != null)
            {
                Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            }
        }

        /// <summary>
        /// 全局下按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            GlobalPageKeyDown(sender, args);
        }
    }
}
