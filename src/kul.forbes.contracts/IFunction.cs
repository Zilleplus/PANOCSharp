using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts
{
    public interface IFunction
    {
        (double cost, Vector<double> gradient) Evaluate(Vector<double> location);
    }
}
