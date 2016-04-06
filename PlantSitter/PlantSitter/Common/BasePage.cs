using JP.Utils.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace PlantSitter_Resp.Common
{
    public abstract class BasePage : Page
    {
        public BasePage()
        {
            this.Loaded += BasePage_Loaded;
            SetUpPageAnimation();
        }

        private void BasePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.DataContext is INavigable)
            {
                var page = this.DataContext as INavigable;
                page.OnLoaded();
            }
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (this.DataContext is INavigable)
            {
                var page = this.DataContext as INavigable;
                page.Deactivate(e);
            }
        }
    }
}
