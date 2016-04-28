using PlantSitter.Common;
using PlantSitter.ViewModel;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Numerics;
using System;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Data;

namespace PlantSitter.View
{
    public sealed partial class ShellPage : BasePage
    {
        public MainViewModel MainVM { get; set; }

        private Compositor _compositor;
        private Visual _drawerGridVisual;
        private Visual _maskVisual;

        public bool IsDrawerOpen
        {
            get { return (bool)GetValue(IsDrawerOpenProperty); }
            set { SetValue(IsDrawerOpenProperty, value); }
        }

        public static readonly DependencyProperty IsDrawerOpenProperty =
            DependencyProperty.Register("IsDrawerOpen", typeof(bool), typeof(ShellPage), new PropertyMetadata(false, OnPropertyChanged));

        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as ShellPage;
            if (!(bool)e.NewValue)
            {
                page.SlideOutDrawer();
            }
            else page.SlideInDrawer();
        }


        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = MainVM = new MainViewModel();
            PrepareComp();
            this.InitBinding();
            NavigationService.ContentFrame = this.ContentFrame;
        }

        private void InitBinding()
        {
            var b = new Binding()
            {
                Source = MainVM,
                Path = new PropertyPath("IsDrawerOpen"),
                Mode = BindingMode.TwoWay,
            };
            this.SetBinding(IsDrawerOpenProperty, b);
        }

        private void PrepareComp()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _drawerGridVisual = ElementCompositionPreview.GetElementVisual(DrawerGrid);
            _maskVisual = ElementCompositionPreview.GetElementVisual(MaskBorder);

            _maskVisual.Opacity = 0;
            MaskBorder.Visibility = Visibility.Collapsed;
            _drawerGridVisual.Offset = new Vector3(-300f, 0f, 0f);
        }

        private void HamBtn_OnClick()
        {
            if (IsDrawerOpen)
            {
                IsDrawerOpen = false;
            }
            else
            {
                IsDrawerOpen = true;
            }
        }

        public void SlideInDrawer()
        {
            MaskBorder.Visibility = Visibility.Visible;

            var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, 1f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(300);

            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, 0f);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(300);

            _maskVisual.StartAnimation("Opacity", fadeAnimation);
            _drawerGridVisual.StartAnimation("Offset.x", offsetAnimation);
        }

        public void SlideOutDrawer()
        {
            var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1f, 0f);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(300);

            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, -300f);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(300);

            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            _maskVisual.StartAnimation("Opacity", fadeAnimation);
            _drawerGridVisual.StartAnimation("Offset.x", offsetAnimation);
            batch.Completed += (sender, e) =>
              {
                  MaskBorder.Visibility = Visibility.Collapsed;
              };
            batch.End();
        }

        private void MaskBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsDrawerOpen)
            {
                IsDrawerOpen = false;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationService.RootFrame.BackStack.Clear();
        }
    }
}
