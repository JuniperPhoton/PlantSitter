using JP.API;
using JP.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace PlantSitter.Common
{
    public static class StatusBarHelper
    {
        public static void SetUpStatusBar()
        {
            if (ApiInformationHelper.HasStatusBar)
            {
                var sb = StatusBar.GetForCurrentView();
                sb.ForegroundColor = Colors.White;
            }
        }
    }
}
