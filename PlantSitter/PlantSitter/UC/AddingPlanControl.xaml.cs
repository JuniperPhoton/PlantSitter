using JP.Utils.Helper;
using JP.UWP.CustomControl;
using PlantSitter.Common;
using PlantSitter.ViewModel;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace PlantSitter.UC
{
    public sealed partial class AddingPlanControl : UserControl
    {
        private AddPlanViewModel AddPlanVM { get; set; }

        private Compositor _compositor;
        private Visual _addGridVisual;
        private Visual _searchResultGridVisual;

        public bool ShowAddGrid
        {
            get { return (bool)GetValue(ShowAddGridProperty); }
            set { SetValue(ShowAddGridProperty, value); }
        }

        public static readonly DependencyProperty ShowAddGridProperty =
            DependencyProperty.Register("ShowAddGrid", typeof(bool), typeof(AddingPlanControl), new PropertyMetadata(false, OnShowAddGridPropertyChanged));

        public static void OnShowAddGridPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AddingPlanControl;
            control.ShowOrHideAddingGrid((bool)e.NewValue);
        }

        public bool ShowSearchResultGrid
        {
            get { return (bool)GetValue(ShowSearchResultGridProperty); }
            set { SetValue(ShowSearchResultGridProperty, value); }
        }

        public static readonly DependencyProperty ShowSearchResultGridProperty =
            DependencyProperty.Register("ShowSearchResultGrid", typeof(bool), typeof(AddingPlanControl), new PropertyMetadata(false, OnShowSearchResultGridPropertyChanged));

        public static void OnShowSearchResultGridPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AddingPlanControl;
            control.ShowOrHideSearchingImgGrid((bool)e.NewValue);
        }

        public AddingPlanControl()
        {
            this.InitializeComponent();

            this.DataContext = AddPlanVM = new AddPlanViewModel();

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _addGridVisual = ElementCompositionPreview.GetElementVisual(AddingGrid);
            _searchResultGridVisual = ElementCompositionPreview.GetElementVisual(SearchImageGrid);

            InitBinding();
        }

        private void InitBinding()
        {
            var b = new Binding()
            {
                Source = AddPlanVM,
                Path = new PropertyPath("ShowAddGrid"),
                Mode = BindingMode.TwoWay,
            };
            this.SetBinding(ShowAddGridProperty, b);

            var b2 = new Binding()
            {
                Source = AddPlanVM,
                Path = new PropertyPath("ShowSearchResultGrid"),
                Mode = BindingMode.TwoWay,
            };
            this.SetBinding(ShowSearchResultGridProperty, b2);
        }

        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateContentLayout();
        }

        private void UpdateContentLayout()
        {
            RootGrid.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight) };
            _addGridVisual.Offset = new Vector3((float)ActualWidth, 0f, 0f);
            _searchResultGridVisual.Offset = new Vector3(0, (float)ActualHeight, 0f);
        }

        public void ShowOrHideAddingGrid(bool show)
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, show ? 0f : (float)ActualWidth);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(500);

            _addGridVisual.StartAnimation("offset.x", offsetAnimation);
        }

        public void ShowOrHideSearchingImgGrid(bool show)
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, show ? 0f : (float)ActualHeight);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(500);

            _searchResultGridVisual.StartAnimation("offset.y", offsetAnimation);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            PopupService.CurrentShownCPEX?.Hide();
            if(NavigationService.RootFrame.CanGoBack)
            {
                NavigationService.RootFrame.GoBack();
            }
        }

        #region ItemAnimation
        private void ListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
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
            var itemsPanel = (ItemsStackPanel)ResultListView.ItemsPanelRoot;
            var itemContainer = (ListViewItem)sender;
            var itemIndex = ResultListView.IndexFromContainer(itemContainer);

            var uc = itemContainer.ContentTemplateRoot as UIElement;

            // Don't animate if we're not in the visible viewport
            if (itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex)
            {
                var itemVisual = ElementCompositionPreview.GetElementVisual(itemContainer);

                float width = (float)uc.RenderSize.Width;
                float height = (float)uc.RenderSize.Height;
                itemVisual.CenterPoint = new Vector3(width / 2, height / 2, 0f);
                itemVisual.Opacity = 0f;
                itemVisual.Offset = new Vector3(0, 100, 0);

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
                itemVisual.StartAnimation("Offset.y", offsetAnimation);
                itemVisual.StartAnimation("Opacity", fadeAnimation);
            }
            itemContainer.Loaded -= ItemContainer_Loaded;
        }

        #endregion

        private void HideResultBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowSearchResultGrid = false;
        }

        private void Op1Btn_Click(object sender, RoutedEventArgs e)
        {
            Op1Img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icon/icon_chooseSun.png"));
            Op2Img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icon/icon_cloud.png"));
            AddPlanVM.CurrentPlant.LikeSunshine = true;
            AddPlanVM.CurrentPlant.LightRange = new Vector2(100f,20000f);
        }

        private void Op2Btn_Click(object sender, RoutedEventArgs e)
        {
            Op1Img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icon/icon_Sun.png"));
            Op2Img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icon/icon_chooseCloud.png"));
            AddPlanVM.CurrentPlant.LikeSunshine = false;
            AddPlanVM.CurrentPlant.LightRange = new Vector2(1f,10000f);
        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateContentLayout();
        }
    }
}
