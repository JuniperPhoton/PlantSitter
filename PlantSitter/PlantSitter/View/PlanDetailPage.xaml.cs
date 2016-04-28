using PlantSitter.Common;
using PlantSitter.ViewModel;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace PlantSitter.View
{
    public sealed partial class PlanDetailPage : BasePage
    {
        private PlanDetailViewModel PlanDetailVM { get; set; }

        private Compositor _compositor;
        private Visual _avatarVisual;
        private Visual _nameVisual;
        private Visual _descVisual;
        private bool _doingAnimation = false;

        public PlanDetailPage()
        {
            this.InitializeComponent();
            this.DataContext = PlanDetailVM = new PlanDetailViewModel();

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _avatarVisual = ElementCompositionPreview.GetElementVisual(AvatarGrid);
            _nameVisual = ElementCompositionPreview.GetElementVisual(NameTB);
            _descVisual = ElementCompositionPreview.GetElementVisual(ScoreDescTB);

            this.SizeChanged += PlanDetailPage_SizeChanged;
        }

        private void PlanDetailPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var nameOffsetX = NameTB.TransformToVisual(RootGrid).TransformPoint(new Point(0, 0)).X;
            var avatarOffsetX = AvatarGrid.TransformToVisual(RootGrid).TransformPoint(new Point(0, 0)).X;
            var descOffsetX = ScoreDescTB.TransformToVisual(RootGrid).TransformPoint(new Point(0, 0)).X;

            if (e.NewSize.Width >= 700)
            {
                if (!_doingAnimation && _nameVisual.Offset.X == 0)
                {
                    var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                    offsetAnimation.InsertKeyFrame(1f, (float)TopContentGrid.ActualWidth/2f - (float)NameTB.ActualWidth/2);
                    offsetAnimation.Duration = TimeSpan.FromMilliseconds(700);

                    var offsetAnimation2 = _compositor.CreateScalarKeyFrameAnimation();
                    offsetAnimation2.InsertKeyFrame(1f, (float)TopContentGrid.ActualWidth / 2f - (float)AvatarGrid.ActualWidth / 2);
                    offsetAnimation2.Duration = TimeSpan.FromMilliseconds(1000);

                    var offsetAnimation3 = _compositor.CreateScalarKeyFrameAnimation();
                    offsetAnimation3.InsertKeyFrame(1f, (float)TopContentGrid.ActualWidth / 2f - (float)ScoreDescTB.ActualWidth / 2);
                    offsetAnimation3.Duration = TimeSpan.FromMilliseconds(1300);

                    var scaleAnimation = _compositor.CreateScalarKeyFrameAnimation();
                    scaleAnimation.InsertKeyFrame(1f,1f);
                    scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);

                    var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                    _nameVisual.StartAnimation("offset.x", offsetAnimation);
                    _avatarVisual.StartAnimation("offset.x", offsetAnimation2);
                    _descVisual.StartAnimation("offset.x", offsetAnimation3);
                    _descVisual.StartAnimation("scale.x", scaleAnimation);
                    _descVisual.StartAnimation("scale.y", scaleAnimation);
                    _doingAnimation = true;
                    batch.Completed += (senderx, ex) =>
                      {
                          _doingAnimation = false;
                      };
                    batch.End();
                }
                else if(!_doingAnimation && _nameVisual.Offset.X!=0)
                {
                    _nameVisual.Offset = new Vector3((float)TopContentGrid.ActualWidth / 2f - (float)NameTB.ActualWidth / 2, 0f, 0f);
                    _avatarVisual.Offset = new Vector3((float)TopContentGrid.ActualWidth / 2f - (float)AvatarGrid.ActualWidth / 2, 0f, 0f);
                    _descVisual.Offset = new Vector3((float)TopContentGrid.ActualWidth / 2f - (float)ScoreDescTB.ActualWidth / 2, 0f, 0f);
                }
            }
            else
            {
                if (_nameVisual.Offset.X != 0 && !_doingAnimation)
                {
                    var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                    offsetAnimation.InsertKeyFrame(1f, 0f);
                    offsetAnimation.Duration = TimeSpan.FromMilliseconds(1000);

                    var scaleAnimation = _compositor.CreateScalarKeyFrameAnimation();
                    scaleAnimation.InsertKeyFrame(1f, 0.7f);
                    scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);

                    var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                    _nameVisual.StartAnimation("offset.x", offsetAnimation);
                    _avatarVisual.StartAnimation("offset.x", offsetAnimation);
                    _descVisual.StartAnimation("offset.x", offsetAnimation);
                    _descVisual.StartAnimation("scale.x", scaleAnimation);
                    _descVisual.StartAnimation("scale.y", scaleAnimation);
                    _doingAnimation = true;
                    batch.Completed += (senderx, ex) =>
                    {
                        _doingAnimation = false;
                    };
                    batch.End();
                }
            }
        }

        protected override void SetupCacheMode()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        protected override void SetUpPageAnimation()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();

            var info = new ContinuumNavigationTransitionInfo();

            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;
        }

        private void AdaptiveGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            int index = args.ItemIndex;
            var root = args.ItemContainer.ContentTemplateRoot as UserControl;
            // Don't run an entrance animation if we're in recycling
            if (!args.InRecycleQueue)
            {
                //args.ItemContainer.Loaded -= ItemContainer_Loaded;
                //args.ItemContainer.Loaded += ItemContainer_Loaded;
            }
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsPanel = (ItemsWrapGrid)DataGridView.ItemsPanelRoot;
            var itemContainer = (GridViewItem)sender;
            var itemIndex = DataGridView.IndexFromContainer(itemContainer);

            var uc = itemContainer.ContentTemplateRoot as UIElement;

            // Don't animate if we're not in the visible viewport
            if (itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex)
            {
                var itemVisual = ElementCompositionPreview.GetElementVisual(itemContainer);

                float width = (float)uc.RenderSize.Width;
                float height = (float)uc.RenderSize.Height;
                itemVisual.CenterPoint = new Vector3(width / 2, height / 2, 0f);
                itemVisual.Opacity = 0f;
                itemVisual.Offset = new Vector3(0, 50, 0);

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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //var grid = sender as Grid;
            //if(grid.ActualWidth>300)
            //{
            //    DataGridView.MinItemHeight = 50;
            //}
            //else
            //{
            //    DataGridView.MinItemHeight = 150;
            //}
        }
    }
}
