using PlantSitter_Resp.Common;
using PlantSitter_Resp.ViewModel;
using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Windows.UI.Xaml.Data;

namespace PlantSitter_Resp.View
{
    public sealed partial class MainPage : BasePage
    {
        public MainViewModel MainVM { get; set; }

        private Compositor _compositor;

        public bool ShowLoginControl
        {
            get { return (bool)GetValue(ShowLoginControlProperty); }
            set { SetValue(ShowLoginControlProperty, value); }
        }

        public static readonly DependencyProperty ShowLoginControlProperty =
            DependencyProperty.Register("ShowLoginControl", typeof(bool), typeof(MainPage), new PropertyMetadata(true,
                (sender,e)=>
                {
                    var page = sender as MainPage;
                    page.PlayLoginControlAnim((bool)e.NewValue);
                }));

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = MainVM = new MainViewModel();
            InitialCompositor();
            InitialBinding();
        }

        private void InitialBinding()
        {
            var b = new Binding()
            {
                Source = MainVM,
                Path = new PropertyPath("ShowLoginControl"),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(this, ShowLoginControlProperty, b);
        }

        private void InitialCompositor()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        private void PlayLoginControlAnim(bool isShown)
        {
            var loginVisual = ElementCompositionPreview.GetElementVisual(LoginControl);
            loginVisual.Offset = isShown ? new Vector3(150f, 0f, 0f) : new Vector3(0, 0, 0);
            loginVisual.Opacity = isShown ? 0f : 1f;

            var fadeAnim = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnim.InsertKeyFrame(1f, isShown? 1f:0f);
            fadeAnim.Duration = TimeSpan.FromMilliseconds(1250);

            var offsetAnim = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnim.InsertKeyFrame(1f, isShown? new Vector3(0, 0, 0):new Vector3(150,0,0));
            offsetAnim.Duration = TimeSpan.FromMilliseconds(1250);

            loginVisual.StartAnimation("Offset", offsetAnim);
            loginVisual.StartAnimation("Opacity", fadeAnim);
        }
    }
}
