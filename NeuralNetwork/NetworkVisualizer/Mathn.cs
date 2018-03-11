using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkVisualizer
{
    public class Mathm
    {
        public static decimal Sweep(decimal percentage, decimal low, decimal high)
        {
            return ((high - low) * percentage) + low;
        }

        public static decimal Normalize(decimal value, decimal low, decimal high)
        {
            return (value - low) / (high - low);
        }

        public static decimal Map(decimal value, decimal low, decimal high, decimal toMin, decimal toMax)
        {
            decimal p = Normalize(value, low, high);
            return (p * (toMax - toMin)) + toMin;
        }
    }
}
