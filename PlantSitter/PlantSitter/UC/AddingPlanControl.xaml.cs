using JP.UWP.CustomControl;
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

namespace PlantSitter.UC
{
    public sealed partial class AddingPlanControl : UserControl
    {
        private AddPlanViewModel AddPlanVM { get; set; }

        private Compositor _compositor;
        private Visual _addGridVisual;
        private Visual _searchGridVisual;

        public bool ShowAddGrid
        {
            get { return (bool)GetValue(ShowAddGridProperty); }
            set { SetValue(ShowAddGridProperty, value); }
        }

        public static readonly DependencyProperty ShowAddGridProperty =
            DependencyProperty.Register("ShowAddGrid", typeof(bool), typeof(AddingPlanControl), new PropertyMetadata(false, OnPropertyChanged));

        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AddingPlanControl;
            if ((bool)e.NewValue)
            {
                control.ShowAddingGrid();
            }
            else control.HideAddGrid();
        }

        public AddingPlanControl()
        {
            this.InitializeComponent();

            this.DataContext = AddPlanVM = new AddPlanViewModel();

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _addGridVisual = ElementCompositionPreview.GetElementVisual(AddingGrid);
            _searchGridVisual = ElementCompositionPreview.GetElementVisual(SearchGrid);

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
        }

        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            RootGrid.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight) };
            _addGridVisual.Offset = new Vector3((float)ActualWidth, 0f, 0f);
        }

        public void ShowAddingGrid()
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, 0f);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(500);

            _addGridVisual.StartAnimation("offset.x", offsetAnimation);
        }

        public void HideAddGrid()
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, (float)ActualWidth);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(500);

            _addGridVisual.StartAnimation("offset.x", offsetAnimation);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            PopupService.CurrentShownCPEX?.Hide();
        }

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

    }
}
