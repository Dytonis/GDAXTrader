using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Synapse
    {
        public Node[] Outputs;
        public Node Input;

        public double Weight;
        public double Value;

        public bool isValid
        {
            get
            {
                if (Outputs.Length > 1 && Input != null)
                    return true;
                else return false;
            }
        }

        public Synapse()
        {

        }
    }
}
