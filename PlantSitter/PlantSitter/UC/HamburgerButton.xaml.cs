using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PlantSitter.UC
{
    public sealed partial class HamburgerButton : UserControl
    {
        public ICommand OnClickCommand
        {
            get { return (ICommand)GetValue(OnClickCommandProperty); }
            set { SetValue(OnClickCommandProperty, value); }
        }

        public static readonly DependencyProperty OnClickCommandProperty =
            DependencyProperty.Register("OnClickCommand", typeof(ICommand), typeof(HamburgerButton), new PropertyMetadata(null));

        public event Action OnClick;

        public HamburgerButton()
        {
            this.InitializeComponent();
        }

        private void HamburgerBtn_Click(object sender, RoutedEventArgs e)
        {
            OnClickCommand?.Execute(null);
            OnClick?.Invoke();
        }
    }
}
