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
    public sealed partial class DrawerControl : UserControl
    {
        public ICommand OnClickItem1Command
        {
            get { return (ICommand)GetValue(OnClickItem1CommandProperty); }
            set { SetValue(OnClickItem1CommandProperty, value); }
        }

        public static readonly DependencyProperty OnClickItem1CommandProperty =
            DependencyProperty.Register("OnClickItem1Command", typeof(ICommand), typeof(DrawerControl), new PropertyMetadata(null));

        public ICommand OnClickItem2Command
        {
            get { return (ICommand)GetValue(OnClickItem2CommandProperty); }
            set { SetValue(OnClickItem2CommandProperty, value); }
        }

        public static readonly DependencyProperty OnClickItem2CommandProperty =
            DependencyProperty.Register("OnClickItem2Command", typeof(ICommand), typeof(DrawerControl), new PropertyMetadata(null));

        public ICommand OnClickItem3Command
        {
            get { return (ICommand)GetValue(OnClickItem3CommandProperty); }
            set { SetValue(OnClickItem3CommandProperty, value); }
        }

        public static readonly DependencyProperty OnClickItem3CommandProperty =
            DependencyProperty.Register("OnClickItem3Command", typeof(ICommand), typeof(DrawerControl), new PropertyMetadata(null));

        public ICommand OnClickItem4Command
        {
            get { return (ICommand)GetValue(OnClickItem4CommandProperty); }
            set { SetValue(OnClickItem4CommandProperty, value); }
        }

        public static readonly DependencyProperty OnClickItem4CommandProperty =
            DependencyProperty.Register("OnClickItem4Command", typeof(ICommand), typeof(DrawerControl), new PropertyMetadata(null));

        public DrawerControl()
        {
            this.InitializeComponent();
        }
    }
}
