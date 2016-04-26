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
            DependencyProperty.Register("ShowAddGrid", typeof(bool), typeof(AddingPlanControl), new PropertyMetadata(false,OnPropertyChanged));

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
    }
}
