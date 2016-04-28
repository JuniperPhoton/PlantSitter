using PlantSitter.Common;
using PlantSitter.ViewModel;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace PlantSitter.View
{
    public sealed partial class AllPlansPage : BasePage
    {
        public UserPlansViewModel UserPlansVM { get; set; }

        private Compositor _compositor;
        private Visual _loadingVisual;
        private Visual _refreshVisual;

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(AllPlansPage), new PropertyMetadata(false, OnLoadingPropertyChanged));

        public static void OnLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as AllPlansPage;
            if (!(bool)e.NewValue)
            {
                page.HideLoading();
            }
            else page.ShowLoading();
        }

        public AllPlansPage()
        {
            this.InitializeComponent();
            this.DataContext = UserPlansVM = new UserPlansViewModel(App.VMLocator.MainVM.CurrentUser);

            var b2 = new Binding()
            {
                Source = UserPlansVM,
                Path = new PropertyPath("IsLoading"),
                Mode = BindingMode.TwoWay,
            };
            this.SetBinding(IsLoadingProperty, b2);

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _loadingVisual = ElementCompositionPreview.GetElementVisual(LoadingGrid);
            _refreshVisual = ElementCompositionPreview.GetElementVisual(RefreshSymbol);
            _loadingVisual.Offset = new Vector3(0f, -50f, 0f);
        }

        

        public void ShowLoading()
        {
            var showAnimation = _compositor.CreateVector3KeyFrameAnimation();
            showAnimation.InsertKeyFrame(1, new Vector3(0f, 50f, 0f));
            showAnimation.Duration = TimeSpan.FromMilliseconds(500);

            var linearEasingFunc = _compositor.CreateLinearEasingFunction();
            
            var rotateAnimation = _compositor.CreateScalarKeyFrameAnimation();
            rotateAnimation.InsertKeyFrame(1, 3600f, linearEasingFunc);
            rotateAnimation.Duration = TimeSpan.FromMilliseconds(10000);
            rotateAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            _refreshVisual.CenterPoint = new Vector3((float)RefreshSymbol.ActualWidth / 2, (float)RefreshSymbol.ActualHeight / 2, 0f);
            _refreshVisual.RotationAngleInDegrees = 0;

            _refreshVisual.StopAnimation("RotationAngleInDegrees");
            _refreshVisual.StartAnimation("RotationAngleInDegrees", rotateAnimation);
            _loadingVisual.StartAnimation("Offset", showAnimation);
        }

        public void HideLoading()
        {
            var showAnimation = _compositor.CreateScalarKeyFrameAnimation();
            showAnimation.InsertKeyFrame(1, -50f);
            showAnimation.Duration = TimeSpan.FromMilliseconds(500);

            _loadingVisual.StartAnimation("Offset.y", showAnimation);
        }

        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            RootGrid.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight) };
        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RootGrid.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight) };
        }
    }
}
