using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts
{
    public interface IProx
    {
        double Prox(Vector<double> vector);
    }
}
