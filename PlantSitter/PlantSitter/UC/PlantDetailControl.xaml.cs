using PlantSitter.Common;
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
            await DownloadImageHelper.DownloadImage(CurrentPlant);
        }

        private void PlantDetailControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(MainScrollViewer);
            var offsetY = (float)NameGrid.TransformToVisual(this.RootGrid).
                                    TransformPoint(new Point(0, 0)).Y;

            Canvas.SetZIndex(NameGrid.Parent as StackPanel, 10);

            var scrollingAnimation = _compositor.CreateExpressionAnimation(
                   "(ScrollingProperties.Translation.Y +OffsetY> 0 ? 0 : -OffsetY - ScrollingProperties.Translation.Y-0f)");
            scrollingAnimation.SetReferenceParameter("ScrollingProperties", scrollProperties);
            scrollingAnimation.SetScalarParameter("OffsetY", offsetY);

            //往下滚动，Translation 负增长
            var fadeAnimation = _compositor.CreateExpressionAnimation();
            fadeAnimation.Expression = "1+(ScrollingProperties.Translation.Y/100f)";
            fadeAnimation.SetReferenceParameter("ScrollingProperties", scrollProperties);

            _avatarGridVisual.StartAnimation("opacity", fadeAnimation);
            _nameGridVisual.StartAnimation("offset.y", scrollingAnimation);
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
