using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Layer : IEnumerable
    {
        private Node[] list;

        public int Count
        {
            get
            {
                return list.Length;
            }
        }

        public int Length
        {
            get
            {
                return list.Length;
            }
        }

        public Node this[int i]
        {
            get
            {
                return list[i];
            }
        }

        public double[] GetLayerValues()
        {
            double[] values = list.Select(x => x.Value).ToArray();
            return values;
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void ForwardPropagation()
        {
            foreach (Node n in list)
                n.Update();
        }
    }
}
