using Microsoft.Graphics.Canvas.UI.Xaml;
using PlantSitterShared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PlantSitter.UC
{
    public sealed partial class DataTableControl : UserControl
    {
        public TableGraphics TableData
        {
            get
            {
                return this.DataContext as TableGraphics;
            }
        }

        public SolidColorBrush DotThemeColor
        {
            get { return (SolidColorBrush)GetValue(DotThemeColorProperty); }
            set { SetValue(DotThemeColorProperty, value); }
        }

        public static readonly DependencyProperty DotThemeColorProperty =
            DependencyProperty.Register("DotThemeColor", typeof(SolidColorBrush), typeof(DataTableControl), new PropertyMetadata(Colors.DarkCyan));


        public SolidColorBrush LineStrokeColor
        {
            get { return (SolidColorBrush)GetValue(LineStrokeColorProperty); }
            set { SetValue(LineStrokeColorProperty, value); }
        }

        public static readonly DependencyProperty LineStrokeColorProperty =
            DependencyProperty.Register("LineStrokeColor", typeof(SolidColorBrush), typeof(DataTableControl), new PropertyMetadata(Colors.Black));

        public float LineStrokeWidth
        {
            get { return (float)GetValue(LineStrokeWidthProperty); }
            set { SetValue(LineStrokeWidthProperty, value); }
        }

        public static readonly DependencyProperty LineStrokeWidthProperty =
            DependencyProperty.Register("LineStrokeWidth", typeof(float), typeof(DataTableControl), new PropertyMetadata(3f));

        public DataTableControl()
        {
            this.InitializeComponent();
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (TableData == null) return;
            if (!sender.ReadyToDraw) return;

            var point1 = new Vector2(120, 220);
            var point2 = new Vector2(210, 240);
            var point3 = new Vector2(316, 235);
            args.DrawingSession.DrawLine(point1,point2, Colors.Black,5f);
            args.DrawingSession.DrawLine(point2,point3,Colors.Black, 5f);
            args.DrawingSession.FillCircle(point1, 20f, Colors.DarkCyan);
            args.DrawingSession.FillCircle(point2, 20f, Colors.DarkCyan);
            args.DrawingSession.FillCircle(point3, 20f, Colors.DarkCyan);
        }

        
    }
}
