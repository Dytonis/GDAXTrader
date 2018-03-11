using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Network : IEnumerable
    {
        private Layer[] center;
        private Layer Inputs;
        private Layer Outputs;

        public IEnumerator GetEnumerator()
        {
            return center.GetEnumerator();
        }

        public Layer GetInputLayer() => Inputs;
        public Layer GetOutputLayer() => Outputs;
        public Layer SetInputLayer(Layer layer) => Inputs = layer;
        public Layer SetOutputLayer(Layer layer) => Outputs = layer;

        public void SetInputValues(double[] values)
        {
            if (values.Length != Inputs.Count)
                throw new InvalidOperationException("Double array 'values' must have equal items as the input layer. Number of inputs: " + values.Length + ", Nodes in input layer: " + Inputs.Count);

            for(int i = 0; i < values.Length; i++)
            {
                Inputs[i].Value = values[i];
            }
        }

        public double[] GetOutputValues()
        {
            return Outputs.GetLayerValues();
        }

        public void ForwardPropagate()
        {
            foreach (Layer l in center)
                foreach(Node n in l)
                    n.Update();

            foreach (Node n in Outputs)
                n.Update();
        }
    }
}
