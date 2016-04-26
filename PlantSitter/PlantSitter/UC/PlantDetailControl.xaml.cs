using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace PlantSitter.UC
{
    public sealed partial class PlantDetailControl : UserControl
    {
        private Compositor _compositor;
        private Visual _avatarGridVisual;

        public PlantDetailControl()
        {
            this.InitializeComponent();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _avatarGridVisual = ElementCompositionPreview.GetElementVisual(AvatarGrid);

            this.SizeChanged += PlantDetailControl_SizeChanged;
        }

        private void PlantDetailControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var properties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(MainScrollViewer);

            var fadeAnimation = _compositor.CreateExpressionAnimation();
            fadeAnimation.Expression = "(1d-p.Translation.Y/10d)>=0?(1d-p.Translation.Y/10d):0";
            fadeAnimation.SetReferenceParameter("p", properties);

            _avatarGridVisual.StartAnimation("Opacity", fadeAnimation);
        }
    }
}
