using JP.Utils.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
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
    public sealed partial class MainItemControl : UserControl
    {
        public MainItemControl()
        {
            this.InitializeComponent();
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var ccl = new CanvasCommandList(sender);
            using (var ds = ccl.CreateDrawingSession())
            {
                ds.FillRectangle(new Rect(10, 10, this.ActualWidth-20, this.ActualHeight-20),Colors.White);

                var shadowEffect = new Transform2DEffect
                {
                    Source = new ShadowEffect
                    {
                        Source = ccl,
                        BlurAmount =3,
                        ShadowColor = Color.FromArgb(60, 0, 0, 0),
                    },
                    BorderMode=EffectBorderMode.Soft,
                    TransformMatrix = Matrix3x2.CreateTranslation(5, 5)
                };
                args.DrawingSession.DrawImage(shadowEffect,0,0);
            }
            
        }
    }
}
