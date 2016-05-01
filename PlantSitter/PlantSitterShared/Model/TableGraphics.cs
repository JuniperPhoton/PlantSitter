using GalaSoft.MvvmLight;
using PlantSitterShared.Enum;
using System.Collections.Generic;
using System.Numerics;

namespace PlantSitterShared.Model
{
    public class TableGraphics : ViewModelBase
    {
        public IEnumerable<PlantTimeline> Data { get; set; }

        public RecordDataKind Kind { get; set; }

        public TableGraphics(IEnumerable<PlantTimeline> data,RecordDataKind kind)
        {
            this.Data = data;
            this.Kind = kind;
        }
    }
}
