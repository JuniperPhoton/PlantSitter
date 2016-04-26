using PlantSitter.Common;
using PlantSitter.ViewModel;
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

namespace PlantSitter.View
{
    public sealed partial class AddPlanPage : BasePage
    {
        public AddPlanViewModel AddPlanVM { get; set; }

        public AddPlanPage()
        {
            this.InitializeComponent();
            this.DataContext = AddPlanVM = new AddPlanViewModel();
        }
    }
}
