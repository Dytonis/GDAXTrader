using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Node
    {
        private double Bias;
        public double Value;

        public SynapseCollection Inputs;

        public void Update()
        {
            Value = Inputs.ForwardPropagation(Bias);
        }
    }
}
