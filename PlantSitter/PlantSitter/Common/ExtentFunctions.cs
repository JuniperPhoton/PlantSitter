using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlantSitter.Common
{
    public static class ExtentFunctions
    {
        public static string ConvertToString(this Vector2 vector)
        {
            return vector.X.ToString() + "~" + vector.Y.ToString();
        }
    }
}
