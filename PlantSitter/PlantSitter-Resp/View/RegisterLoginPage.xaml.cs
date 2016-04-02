using JP.Utils.Framework;
using PlantSitter_Resp.Common;
using PlantSitter_Resp.ViewModel;
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


namespace PlantSitter_Resp.View
{
    public sealed partial class RegisterLoginPage : BasePage
    {
        public LoginViewModel LoginVM { get; set; }

        public RegisterLoginPage()
        {
            this.InitializeComponent();
            this.DataContext = LoginVM = new LoginViewModel();
        }
    }
}
