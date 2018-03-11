using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Math
    {
        public const double e = 2.718281828;

        public static double Sigmoid(double d)
        {
            double ePow = System.Math.Pow(e, d);
            double num = ePow;
            double den = ePow + 1;
            return num / den;
        }
    }
}
