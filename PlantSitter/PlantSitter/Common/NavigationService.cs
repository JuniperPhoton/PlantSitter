using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PlantSitter.Common
{
    public static class NavigationService
    {
        public static void NavigateViaRootFrame(Type pageType,object param=null)
        {
            RootFrame.Navigate(pageType, param);
        }

        public static Frame RootFrame
        {
            get
            {
                return Window.Current.Content as Frame;
            }
        }

        public static Frame ContentFrame { get; set; }
    }
}
