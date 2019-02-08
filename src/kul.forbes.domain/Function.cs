using kul.forbes.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.domain
{
    public class Function : IFunction
    {
        public (double cost, Vector<double> gradient) Evaluate(Vector<double> location)
        {
            throw new NotImplementedException();
        }
    }
}
