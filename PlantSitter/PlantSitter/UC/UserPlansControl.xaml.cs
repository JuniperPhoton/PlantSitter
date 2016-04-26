using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace PlantSitter.UC
{
    public sealed partial class UserPlansControl : UserControl
    {
        private Compositor _compositor;

        public UserPlansControl()
        {
            this.InitializeComponent();
            this.Loaded += UserPlansControl_Loaded;
        }

        private void UserPlansControl_Loaded(object sender, RoutedEventArgs e)
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        private void AdaptiveGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            int index = args.ItemIndex;
            var root = args.ItemContainer.ContentTemplateRoot as UserControl;
            // Don't run an entrance animation if we're in recycling
            if (!args.InRecycleQueue)
            {
                args.ItemContainer.Loaded -= ItemContainer_Loaded;
                args.ItemContainer.Loaded += ItemContainer_Loaded;
            }
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsPanel = (ItemsWrapGrid)PlansGridView.ItemsPanelRoot;
            var itemContainer = (GridViewItem)sender;
            var itemIndex = PlansGridView.IndexFromContainer(itemContainer);

            var uc = itemContainer.ContentTemplateRoot as UIElement;

            // Don't animate if we're not in the visible viewport
            if (itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex)
            {
                var itemVisual = ElementCompositionPreview.GetElementVisual(itemContainer);

                float width = (float)uc.RenderSize.Width;
                float height = (float)uc.RenderSize.Height;
                itemVisual.CenterPoint = new Vector3(width / 2, height / 2, 0f);
                itemVisual.Opacity = 0f;
                itemVisual.Offset = new Vector3(100, 0, 0);

                // Create KeyFrameAnimations
                var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.InsertExpressionKeyFrame(1f, "0");
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(1000);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(itemIndex * 200);

                var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.InsertExpressionKeyFrame(1f, "1");
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(1000);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(itemIndex * 200);

                // Start animations
                itemVisual.StartAnimation("Offset.x", offsetAnimation);
                itemVisual.StartAnimation("Opacity", fadeAnimation);
            }
            itemContainer.Loaded -= ItemContainer_Loaded;
        }
    }
}
