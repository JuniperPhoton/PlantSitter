using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Numerics;

namespace PlantSitterShared.Model
{
    public class TableGraphics : ViewModelBase
    {
        public IEnumerable<PlantTimeline> Data { get; set; }

        public Vector2 XaxisRange { get; set; }

        public Vector2 YaxisRange { get; set; }

        public TableGraphics(IEnumerable<PlantTimeline> data,Vector2 xrange,Vector2 yrange)
        {
            this.Data = data;
            this.XaxisRange = xrange;
            this.YaxisRange = yrange;
        }
    }
}
