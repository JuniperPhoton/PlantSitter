using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlantSitterShared.API
{
    public static class CTSFactory
    {
        public static CancellationTokenSource MakeCTS(int? timeout)
        {
            return new CancellationTokenSource(timeout==null?10000,timeout.Value);
        }

        public static CancellationTokenSource MakeCTS()
        {
            return new CancellationTokenSource();
        }
    }
}
