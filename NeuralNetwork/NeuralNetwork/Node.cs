using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Node
    {
        public double Weight;
        public double Value;

        public SynapseCollection Inputs;
        public SynapseCollection Outputs;

        public Node(float weight)
        {
            this.Weight = weight;
        }

        public void Update()
        {
            Value = Inputs.GetAverage();
        }
    }
}
