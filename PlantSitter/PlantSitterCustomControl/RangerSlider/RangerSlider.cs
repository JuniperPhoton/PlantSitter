using JP.Utils.UI;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace PlantSitterCustomControl
{
    public class RangerSlider : Control
    {
        #region DP
        /// <summary>
        /// 最小值
        /// </summary>
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(RangerSlider), new PropertyMetadata(0));

        /// <summary>
        /// 最大值
        /// </summary>
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(RangerSlider), new PropertyMetadata(100));

        /// <summary>
        /// 拖动按钮样式的自定义
        /// </summary>
        public DataTemplate ThumbTemplate
        {
            get { return (DataTemplate)GetValue(ThumbTemplateProperty); }
            set { SetValue(ThumbTemplateProperty, value); }
        }

        public static readonly DependencyProperty ThumbTemplateProperty =
            DependencyProperty.Register("ThumbTemplate", typeof(DataTemplate), typeof(RangerSlider), new PropertyMetadata(null));

        /// <summary>
        /// 拖动按钮上字体的颜色
        /// </summary>
        public SolidColorBrush ThemeForeColor
        {
            get { return (SolidColorBrush)GetValue(ThemeForeColorProperty); }
            set { SetValue(ThemeForeColorProperty, value); }
        }

        public static readonly DependencyProperty ThemeForeColorProperty =
            DependencyProperty.Register("ThemeForeColor", typeof(SolidColorBrush), typeof(RangerSlider), new PropertyMetadata(Colors.White));

        /// <summary>
        /// 主题颜色，用于下方表示选择的范围
        /// </summary>
        public SolidColorBrush ThemeColor
        {
            get { return (SolidColorBrush)GetValue(ThemeColorProperty); }
            set { SetValue(ThemeColorProperty, value); }
        }

        public static readonly DependencyProperty ThemeColorProperty =
            DependencyProperty.Register("ThemeColor", typeof(SolidColorBrush), typeof(RangerSlider), new PropertyMetadata(Colors.Black));

        /// <summary>
        /// 当前选择的最小值
        /// </summary>
        public int CurrentMinValue
        {
            get { return (int)GetValue(CurrentMinValueProperty); }
            set { SetValue(CurrentMinValueProperty, value); }
        }

        public static readonly DependencyProperty CurrentMinValueProperty =
            DependencyProperty.Register("CurrentMinValue", typeof(int), typeof(RangerSlider), new PropertyMetadata(30));

        /// <summary>
        /// 当前选择的最大值
        /// </summary>
        public int CurrentMaxValue
        {
            get { return (int)GetValue(CurrentMaxValueProperty); }
            set { SetValue(CurrentMaxValueProperty, value); }
        }

        public static readonly DependencyProperty CurrentMaxValueProperty =
            DependencyProperty.Register("CurrentMaxValue", typeof(int), typeof(RangerSlider), new PropertyMetadata(70));

        /// <summary>
        /// 刻度里的刻度数目，默认为5
        /// </summary>
        public int DivideCount
        {
            get { return (int)GetValue(DivideCountProperty); }
            set { SetValue(DivideCountProperty, value); }
        }

        public static readonly DependencyProperty DivideCountProperty =
            DependencyProperty.Register("DivideCount", typeof(int), typeof(RangerSlider), new PropertyMetadata(5));

        #endregion

        public event Action<Vector2> OnValueChanging;
        public event Action<Vector2> OnValueChanged;

        private SolidColorBrush _darkColor = new SolidColorBrush(ColorConverter.HexToColor("#CCA0A0A0").Value);

        private TextBlock _minValueTB;
        private TextBlock _maxValueTB;
        private Grid _minManiGrid;
        private Grid _maxManiGrid;
        private Canvas _progressCanvas;
        private Canvas _bottomCanvas;
        private Grid _progressGrid;
        private Border _rangeBorder;

        public RangerSlider()
        {
            DefaultStyleKey = (typeof(RangerSlider));
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Init();
        }

        /// <summary>
        /// 初始化，找到 Template 里的控件
        /// </summary>
        private void Init()
        {
            _minManiGrid = (Grid)GetTemplateChild("MinThumbMovingGrid");
            _maxManiGrid = (Grid)GetTemplateChild("MaxThumbMovingGrid");
            _minValueTB = (TextBlock)GetTemplateChild("MinValueTB");
            _maxValueTB = (TextBlock)GetTemplateChild("MaxValueTB");
            _progressCanvas = (Canvas)GetTemplateChild("ProgressCanvas");
            _bottomCanvas = (Canvas)GetTemplateChild("BottomCanavs");
            _progressGrid = (Grid)GetTemplateChild("ProgressGrid");

            _minValueTB.Text = "0";
            _maxValueTB.Text = "100";

            _minManiGrid.ManipulationDelta += _minManiGrid_ManipulationDelta;
            _maxManiGrid.ManipulationDelta += _maxManiGrid_ManipulationDelta;
            _minManiGrid.ManipulationCompleted += _manipulationCompleted;
            _maxManiGrid.ManipulationCompleted += _manipulationCompleted;

            if(!DesignMode.DesignModeEnabled)
            {
                this.Loaded += RangerSlider_Loaded;
                this.SizeChanged += RangerSlider_SizeChanged;
            }
        }

        /// <summary>
        /// 移动”最大块“的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _maxManiGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var minOffsetX = Canvas.GetLeft(_minManiGrid);
            var maxOffsetX = Canvas.GetLeft(_maxManiGrid);
            var newMaxOffsetX = maxOffsetX + e.Delta.Translation.X;
            if (newMaxOffsetX > minOffsetX && newMaxOffsetX<= _progressCanvas.ActualWidth)
            {
                Canvas.SetLeft(_maxManiGrid, newMaxOffsetX);
            }
            DrawRangeBorder();
            UpdateIndicator();
        }

        /// <summary>
        /// 移动”最小块“的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _minManiGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var minOffsetX = Canvas.GetLeft(_minManiGrid);
            var maxOffsetX = Canvas.GetLeft(_maxManiGrid);
            var newMinOffsetX = minOffsetX + e.Delta.Translation.X;
            if (newMinOffsetX < maxOffsetX && newMinOffsetX >= 0)
            {
                Canvas.SetLeft(_minManiGrid, newMinOffsetX);
            }
            DrawRangeBorder();
            UpdateIndicator();
        }

        /// <summary>
        /// 移动完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _manipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            OnValueChanged?.Invoke(new Vector2((float)CurrentMinValue, (float)CurrentMaxValue));
        }

        /// <summary>
        /// 使得最大值与最小值之间高亮
        /// </summary>
        private void DrawRangeBorder()
        {
            if (_rangeBorder == null)
            {
                _rangeBorder = new Border();
            }
            else
            {
                _progressCanvas.Children.Remove(_rangeBorder);
            }

            _rangeBorder.Height = 2;
            _rangeBorder.Background = ThemeColor;

            _progressCanvas.Children.Add(_rangeBorder);

            var minOffsetX = _minManiGrid.TransformToVisual(_progressCanvas).TransformPoint(new Point(_minManiGrid.ActualWidth / 2, 0)).X;
            var maxOffsetX = _maxManiGrid.TransformToVisual(_progressCanvas).TransformPoint(new Point(_maxManiGrid.ActualWidth / 2, 0)).X;
            _rangeBorder.Width = maxOffsetX - minOffsetX;
            Canvas.SetLeft(_rangeBorder, minOffsetX);
        }

        /// <summary>
        /// 更新拖动”块“上的指示器数字
        /// </summary>
        private void UpdateIndicator()
        {
            var minOffsetX = _minManiGrid.TransformToVisual(_progressCanvas).TransformPoint(new Windows.Foundation.Point(_minManiGrid.ActualWidth / 2, 0)).X;
            var maxOffsetX = _maxManiGrid.TransformToVisual(_progressCanvas).TransformPoint(new Windows.Foundation.Point(_maxManiGrid.ActualWidth / 2, 0)).X;

            CurrentMinValue = (int)(MinValue + (minOffsetX / _progressCanvas.ActualWidth) * (MaxValue - MinValue));
            CurrentMaxValue = (int)Math.Ceiling((MinValue + (maxOffsetX / _progressCanvas.ActualWidth) * (MaxValue - MinValue)));

            _minValueTB.Text = CurrentMinValue.ToString();
            _maxValueTB.Text = CurrentMaxValue.ToString();

            OnValueChanging?.Invoke(new Vector2((float)CurrentMinValue, (float)CurrentMaxValue));
        }

        private void RangerSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayoutContent();
        }

        private void RangerSlider_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayoutContent();
        }

        private void UpdateLayoutContent()
        {
            if (this.ActualWidth <= 0 || this.ActualHeight <= 0)
            {
                return;
            }
            var thumbWidth = _maxManiGrid.ActualWidth;
            _bottomCanvas.Margin = new Thickness(thumbWidth / 2, 0, thumbWidth / 2, 0);
            _progressGrid.Margin = new Thickness(thumbWidth / 2, 0, thumbWidth / 2, 0);

            double rangeValue = MaxValue - MinValue;
            var colCount = 5d;
            var valuePerCol = rangeValue / colCount;

            _bottomCanvas.Children.Clear();
            _progressCanvas.Children.Clear();
            for (var i = 0; i < 6; i++)
            {
                var textblock = new TextBlock()
                {
                    Text = (MinValue + i * valuePerCol).ToString(),
                    Foreground = _darkColor,
                };
                _bottomCanvas.Children.Add(textblock);
                Canvas.SetLeft(textblock, ((i / 5d) * _bottomCanvas.ActualWidth) - 5);

                var breakPoint = new Border()
                {
                    Width = 2,
                    Height = 2,
                    Background = new SolidColorBrush(Colors.Black),
                };
                _progressCanvas.Children.Add(breakPoint);
                Canvas.SetLeft(breakPoint, ((i / 5d) * _progressCanvas.ActualWidth));
            }

            _minValueTB.Text = CurrentMinValue.ToString(); ;
            _maxValueTB.Text = CurrentMaxValue.ToString();
            Canvas.SetLeft(_minManiGrid, ((CurrentMinValue - MinValue) / rangeValue) * _progressCanvas.ActualWidth);
            Canvas.SetLeft(_maxManiGrid, ((CurrentMaxValue - MinValue) / rangeValue) * _progressCanvas.ActualWidth);

            DrawRangeBorder();
        }
    }
}
