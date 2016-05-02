using JP.API;
using JP.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace PlantSitter.Common
{
    public static class StatusBarHelper
    {
        public static void SetUpStatusBar()
        {
            if (DeviceHelper.IsMobile)
            {
                var sb = StatusBar.GetForCurrentView();
                sb.ForegroundColor = Colors.White;
                sb.BackgroundOpacity = 1;
                sb.BackgroundColor = (App.Current.Resources["PlantSitterThemeLightColor"] as SolidColorBrush).Color;
            }
        }
    }
}
