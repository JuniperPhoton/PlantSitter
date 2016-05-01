using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PlantSitterResp.UC
{
    public sealed partial class DashboardControl : UserControl
    {
        private Compositor _compositor;
        private Visual _triVisual;

        public DashboardControl()
        {
            this.InitializeComponent();
            this._compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            this._triVisual = ElementCompositionPreview.GetElementVisual(SelectedTriImage);
            this._triVisual.Opacity = 0;

            this.SizeChanged += DashboardControl_SizeChanged;
        }

        private void DashboardControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSelectedImagePos();
        }

        private void PlansGirdView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedImagePos();
        }

        private void UpdateSelectedImagePos()
        {
            var index = PlansGirdView.SelectedIndex;
            if (index < 0) return;
            var container = PlansGirdView.ContainerFromIndex(index) as GridViewItem;
            var centralOffsetX = (float)(container.TransformToVisual(this.RootGrid).TransformPoint(new Point(container.ActualWidth / 2, container.ActualHeight)).X);
            _triVisual.Opacity = 1;
            _triVisual.Offset = new Vector3(centralOffsetX - 15f, -5f, 0f);
        }
    }
}
