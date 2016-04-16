using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace PlantSitterResp.Common
{
    public static class TitleBarHelper
    {
        public static void SetupDarkTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = (App.Current.Resources["PlantSitterDarkColor"] as SolidColorBrush).Color;
            titleBar.ForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = titleBar.BackgroundColor;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = (App.Current.Resources["PlantSitterDarkColor"] as SolidColorBrush).Color;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonInactiveBackgroundColor = titleBar.BackgroundColor;
            titleBar.ButtonInactiveForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = (App.Current.Resources["PlantSitterDarkLightColor"] as SolidColorBrush).Color;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = (App.Current.Resources["PlantSitterDarkDarkColor"] as SolidColorBrush).Color;
        }

        public static void SetupThemeTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = (App.Current.Resources["PlantSitterDashboardDarkColor"] as SolidColorBrush).Color;
            titleBar.ForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = titleBar.BackgroundColor;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = (App.Current.Resources["PlantSitterDashboardDarkColor"] as SolidColorBrush).Color;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonInactiveBackgroundColor = titleBar.BackgroundColor;
            titleBar.ButtonInactiveForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = (App.Current.Resources["PlantSitterDashboardLightColor"] as SolidColorBrush).Color;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = (App.Current.Resources["PlantSitterDashboardDarkColor"] as SolidColorBrush).Color;
        }
    }
}
