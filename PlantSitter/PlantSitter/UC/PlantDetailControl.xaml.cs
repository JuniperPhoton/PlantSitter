using PlantSitterShared.Model;
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
        private Visual _nameGridVisual;

        private Plant CurrentPlant
        {
            get
            {
                return DataContext as Plant;
            }
        }

        public PlantDetailControl()
        {
            this.InitializeComponent();

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _avatarGridVisual = ElementCompositionPreview.GetElementVisual(AvatarGrid);
            _nameGridVisual = ElementCompositionPreview.GetElementVisual(NameGrid);

            this.SizeChanged += PlantDetailControl_SizeChanged;
            this.Loaded += PlantDetailControl_Loaded;
        }

        private async void PlantDetailControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CurrentPlant.UpdateInfoAsync();
        }

        private void PlantDetailControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var properties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(MainScrollViewer);

            var stickAnimation = _compositor.CreateExpressionAnimation();
            stickAnimation.Expression = "(1-p.Translation.Y/10d)>=0?(1d-p.Translation.Y/10d):0";
            stickAnimation.SetReferenceParameter("p", properties);

            //_avatarGridVisual.StartAnimation("Opacity", fadeAnimation);
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
