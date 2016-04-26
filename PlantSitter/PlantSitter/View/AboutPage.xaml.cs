using PlantSitter.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PlantSitter.ViewModel;

namespace PlantSitter.View
{
    public sealed partial class AboutPage : BasePage
    {
        private AboutViewModel AboutVM { get; set; }

        public AboutPage()
        {
            this.InitializeComponent();
            this.DataContext = AboutVM = new AboutViewModel();
        }
    }
}
