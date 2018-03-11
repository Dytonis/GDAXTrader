using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class SynapseCollection : IEnumerable, IDisposable
    {
        private Synapse[] list;

        public void Dispose()
        {
            list = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return list.GetEnumerator();
        }

        public double ForwardPropagation(double Bias)
        {
            List<double> items = new List<double>();

            foreach(Synapse s in list)
            {
                items.Add(s.Weight * s.Input.Value);
            }

            items.Add(Bias);

            double sum = items.Sum();
            return Math.Sigmoid(sum);
        }
    }
}
