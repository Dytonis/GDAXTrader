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
        private int index;
        private Synapse[] list;

        public void Dispose()
        {
            list = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return list.GetEnumerator();
        }

        public double GetAverage()
        {
            double adding = 0;

            foreach(Synapse s in list)
            {
                adding += s.Value;
            }

            return adding / (double)list.Length;
        }
    }
}
