using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using PlantSitterShared.Enum;
using PlantSitterShared.Model;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PlantSitter.UC
{
    public sealed partial class DataTableControl : UserControl
    {
        public TableGraphics TableData
        {
            get { return (TableGraphics)GetValue(TableDataProperty); }
            set { SetValue(TableDataProperty, value); }
        }

        public static readonly DependencyProperty TableDataProperty =
            DependencyProperty.Register("TableData", typeof(TableGraphics), typeof(DataTableControl), new PropertyMetadata(null,
                (sender, e) =>
                {
                    var control = sender as DataTableControl;
                    if (e.NewValue != null)
                        control.Redraw();
                }));

        public float AxisCornerGap => 30f;

        private ICanvasImage _bitmapTiger;

        public DataTableControl()
        {
            this.InitializeComponent();
        }

        public void Redraw()
        {
            CanvasControl.Invalidate();
        }

        private void CanvasControl_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        private async Task CreateResourcesAsync(CanvasControl sender)
        {
            _bitmapTiger = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Backgrd/web.png"));
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (TableData == null) return;

            using (args.DrawingSession)
            {
                var yRange = GetYAxisRange();
                var vectors = new Vector2[TableData.Data.Count()];

                var yEndPoint = new Vector2(AxisCornerGap, AxisCornerGap);
                var zeroPoint = new Vector2(AxisCornerGap, (float)(CanvasControl.Size.Height - AxisCornerGap));
                var xEndPoint = new Vector2((float)(CanvasControl.Size.Width - AxisCornerGap), (float)(CanvasControl.Size.Height - AxisCornerGap));

                //Draw point
                for (int i = 0; i < TableData.Data.Count(); i++)
                {
                    var item = TableData.Data.ElementAt(i);
                    var value = GetValueOfSpecifiedKind(item);
                    var offsetY = GetYPosBaseOnValue(yRange, (float)value);
                    var offsetX = GetXPosBaseOnValue(i);

                    vectors[i] = new Vector2(offsetX, offsetY);
                    args.DrawingSession.FillCircle(vectors[i], 4f, Colors.Black);
                    args.DrawingSession.DrawText(value.ToString(), new Vector2(vectors[i].X - 20f, vectors[i].Y - 20f), Colors.Black, new CanvasTextFormat() { FontSize = 12 });
                    args.DrawingSession.DrawText(item.RecordTime.ToString("MM/dd HH:mm"), new Rect(offsetX - 20f, zeroPoint.Y + 5f, 20f, 50f), Colors.Black, new CanvasTextFormat() { FontSize = 8, WordWrapping = CanvasWordWrapping.WholeWord });
                }

                //Draw line
                for (int i = 0; i < vectors.Count() - 1; i++)
                {
                    args.DrawingSession.DrawLine(vectors[i], vectors[i + 1], Colors.Black, 1f);
                }

                //Draw axis
                args.DrawingSession.DrawLine(yEndPoint, zeroPoint, Colors.Black, 1f);
                args.DrawingSession.DrawLine(zeroPoint, xEndPoint, Colors.Black, 1f);
                args.DrawingSession.DrawLine(yEndPoint, new Vector2(yEndPoint.X - 5, yEndPoint.Y + 5), Colors.Black, 1f);
                args.DrawingSession.DrawLine(yEndPoint, new Vector2(yEndPoint.X + 5, yEndPoint.Y + 5), Colors.Black, 1f);
                args.DrawingSession.DrawLine(xEndPoint, new Vector2(xEndPoint.X - 5, xEndPoint.Y - 5), Colors.Black, 1f);
                args.DrawingSession.DrawLine(xEndPoint, new Vector2(xEndPoint.X - 5, xEndPoint.Y + 5), Colors.Black, 1f);

                //Draw index mark
                args.DrawingSession.DrawText(yRange.Y.ToString(), new Vector2(yEndPoint.X - 30f, yEndPoint.Y + 15f), Colors.Black, new CanvasTextFormat() { FontSize = 12 });
                args.DrawingSession.DrawText(yRange.X.ToString(), new Vector2(zeroPoint.X - 30f, zeroPoint.Y - 15f), Colors.Black, new CanvasTextFormat() { FontSize = 12 });
                args.DrawingSession.DrawText(GetYUnit(), new Vector2(5f, 5f), Colors.Black, new CanvasTextFormat() { FontSize = 12 });
                args.DrawingSession.DrawText(GetXUnit(), new Vector2(xEndPoint.X + 5f, xEndPoint.Y), Colors.Black, new CanvasTextFormat() { FontSize = 12 });

                //Draw background image
                args.DrawingSession.DrawImage(_bitmapTiger);
            }
        }

        private Vector2 GetYAxisRange()
        {
            var dataToDisplay = TableData.Data;
            var maxValue = dataToDisplay.Max(s =>
            {
                return (float)GetValueOfSpecifiedKind(s);
            });
            var minValue = dataToDisplay.Min(s =>
            {
                return (float)GetValueOfSpecifiedKind(s);
            });
            var yAxisMax = maxValue * 1.2;
            var yAixMin = minValue * 0.8;
            return new Vector2((float)yAixMin, (float)yAxisMax);
        }

        private Vector2 GetXAxisRange()
        {
            var dataToDisplay = TableData.Data;
            var maxValue = dataToDisplay.Max(s =>
            {
                if (TableData.Kind == RecordDataKind.EnviMoisture)
                {
                    return s.EnviMoisture;
                }
                else return -1d;
            });
            var minValue = dataToDisplay.Max(s =>
            {
                if (TableData.Kind == RecordDataKind.EnviMoisture)
                {
                    return s.EnviMoisture;
                }
                else return -1d;
            });
            var yAxisMax = maxValue * 1.2;
            var yAixMin = minValue * 0.8;
            return new Vector2((float)yAixMin, (float)yAxisMax);
        }

        private double GetValueOfSpecifiedKind(PlantTimeline s)
        {
            if (TableData.Kind == RecordDataKind.EnviMoisture)
            {
                return s.EnviMoisture;
            }
            else if (TableData.Kind == RecordDataKind.EnviTemp)
            {
                return s.EnviTemp;
            }
            else if (TableData.Kind == RecordDataKind.Light)
            {
                return s.Light;
            }
            else return s.SoilMoisture;
        }

        private float GetYPosBaseOnValue(Vector2 range, float currentValue)
        {
            var ratio = 1f - (currentValue - range.X) / (range.Y - range.X);
            return (float)((CanvasControl.Size.Height-AxisCornerGap*2) * ratio);
        }

        private float GetXPosBaseOnValue(int index)
        {
            return (float)((CanvasControl.Size.Width - AxisCornerGap*2) / TableData.Data.Count() * (index + 1)) + 20f;
        }

        private string GetXUnit()
        {
            return "时间";
        }

        private string GetYUnit()
        {
            switch (TableData.Kind)
            {
                case RecordDataKind.EnviMoisture:
                    {
                        return "%";
                    };
                case RecordDataKind.EnviTemp:
                    {
                        return "摄氏度";
                    };
                case RecordDataKind.Light:
                    {
                        return "Lux";
                    };
                case RecordDataKind.SoilMoisture:
                    {
                        return "";
                    };
                default:
                    {
                        return "";
                    }
            }
        }
    }
}
