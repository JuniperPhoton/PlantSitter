using JP.Utils.Helper;
using JP.Utils.UI;
using PlantSitter.Common;
using PlantSitter.ViewModel;
using System;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
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
        private Visual _topBottomBorderVisual;
        private Visual _topBottomGridVisual;
        private Visual _backgrdVisual;
        private ScrollViewer _mainScrollViewer;
        private Visual _tableViewGrid;
        private bool _doingAnimation = false;

        public bool ShowTableView
        {
            get { return (bool)GetValue(ShowTableViewProperty); }
            set { SetValue(ShowTableViewProperty, value); }
        }

        public static readonly DependencyProperty ShowTableViewProperty =
            DependencyProperty.Register("ShowTableView", typeof(bool), typeof(PlanDetailPage), new PropertyMetadata(false,OnShowTableViewPropertyChanged));

        public static void OnShowTableViewPropertyChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            var page = d as PlanDetailPage;
            page.ShowOrHideTableView((bool)e.NewValue);
        }

        public PlanDetailPage()
        {
            this.InitializeComponent();
            this.DataContext = PlanDetailVM = new PlanDetailViewModel();

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _avatarVisual = ElementCompositionPreview.GetElementVisual(AvatarGrid);
            _nameVisual = ElementCompositionPreview.GetElementVisual(NameTB);
            _descVisual = ElementCompositionPreview.GetElementVisual(ScoreDescTB);
            _topBottomBorderVisual = ElementCompositionPreview.GetElementVisual(TopBottomBorder);
            _topBottomGridVisual = ElementCompositionPreview.GetElementVisual(TopBottomGrid);
            _backgrdVisual = ElementCompositionPreview.GetElementVisual(BackBorder);
            _tableViewGrid = ElementCompositionPreview.GetElementVisual(TableViewGrid);

            _topBottomBorderVisual.Opacity = 0f;
            _tableViewGrid.Offset = new Vector3(0f, (float)Window.Current.Bounds.Height, 0f);

            this.SizeChanged += PlanDetailPage_SizeChanged;
            this.Loaded += PlanDetailPage_Loaded;
            InitBinding();
        }

        private void InitBinding()
        {
            var b = new Binding()
            {
                Source = this.PlanDetailVM,
                Path = new PropertyPath("ShowTableView"),
                Mode = BindingMode.TwoWay
            };
            this.SetBinding(ShowTableViewProperty, b);
        }

        private void PlanDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ActualWidth < 700)
            {
                _descVisual.Scale = new Vector3(0.7f, 0.7f, 1f);
            }
        }

        private void ScoreSP_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScoreDescSP.Margin = new Thickness(20, 0, 20, 20);
        }

        public void ShowOrHideTableView(bool show)
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f,show?0f:(float)Window.Current.Bounds.Height);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(500);
            _tableViewGrid.StartAnimation("offset.y", offsetAnimation);
        }

        private void PlanDetailPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var nameOffsetX = (float)NameTB.TransformToVisual(RootGrid).TransformPoint(new Point(0, 0)).X;
            var avatarOffsetX = (float)AvatarGrid.TransformToVisual(RootGrid).TransformPoint(new Point(0, 0)).X;
            var descOffsetX = (float)ScoreDescTB.TransformToVisual(RootGrid).TransformPoint(new Point(0, 0)).X;

            var centralNameX = (float)RootGrid.ActualWidth / 2 - (float)NameTB.ActualWidth / 2;
            var centralAvatarX = (float)RootGrid.ActualWidth / 2 - (float)AvatarGrid.ActualWidth / 2;
            var centralDescX = (float)RootGrid.ActualWidth / 2 - (float)ScoreDescTB.ActualWidth / 2;

            Debug.WriteLine(centralNameX);

            if (e.NewSize.Width >= 700)
            {
                if (!_doingAnimation && _nameVisual.Offset.X == 0)
                {
                    var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                    offsetAnimation.InsertKeyFrame(1f, centralNameX);
                    offsetAnimation.Duration = TimeSpan.FromMilliseconds(700);

                    var offsetAnimation2 = _compositor.CreateScalarKeyFrameAnimation();
                    offsetAnimation2.InsertKeyFrame(1f, centralAvatarX);
                    offsetAnimation2.Duration = TimeSpan.FromMilliseconds(1000);

                    var offsetAnimation3 = _compositor.CreateScalarKeyFrameAnimation();
                    offsetAnimation3.InsertKeyFrame(1f, centralDescX);
                    offsetAnimation3.Duration = TimeSpan.FromMilliseconds(1300);

                    var scaleAnimation = _compositor.CreateScalarKeyFrameAnimation();
                    scaleAnimation.InsertKeyFrame(1f, 1f);
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
                else if (!_doingAnimation && _nameVisual.Offset.X != 0)
                {
                    _nameVisual.Offset = new Vector3(centralNameX, 0f, 0f);
                    _avatarVisual.Offset = new Vector3(centralAvatarX, 0f, 0f);
                    _descVisual.Offset = new Vector3(centralDescX, 0f, 0f);
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
            UpadateScrollingAnimation();
        }

        private void UpadateScrollingAnimation()
        {
            if (_mainScrollViewer == null)
            {
                _mainScrollViewer = DataGridView.GetScrollViewer();
            }

            var scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_mainScrollViewer);
            var offsetY = (float)ScoreDescSP.TransformToVisual(this.RootGrid).
                                    TransformPoint(new Point(0, 0)).Y;

            var header = DataGridView.Header as FrameworkElement;
            var headerContainer = header.Parent as ContentControl;
            Canvas.SetZIndex(headerContainer, 10);

            var scrollingAnimation = _compositor.CreateExpressionAnimation();
            scrollingAnimation.Expression = "-(prop.Translation.Y) / 150f";
            scrollingAnimation.SetReferenceParameter("prop", scrollProperties);

            _topBottomBorderVisual.StartAnimation("Opacity", scrollingAnimation);
            _backgrdVisual.StartAnimation("Opacity", scrollingAnimation);

            var scrollingAnimation2 = _compositor.CreateExpressionAnimation(
                   "(ScrollingProperties.Translation.Y +OffsetY> 0 ? 0 : -OffsetY - ScrollingProperties.Translation.Y-0f)");
            scrollingAnimation2.SetReferenceParameter("ScrollingProperties", scrollProperties);
            scrollingAnimation2.SetScalarParameter("OffsetY", offsetY);

            _topBottomGridVisual.StartAnimation("offset.y", scrollingAnimation2);
        }

        protected override void SetupCacheMode()
        {
            NavigationCacheMode = NavigationCacheMode.Enabled;
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
    }
}
