using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts
{
    public interface IPanoc
    {
        Vector<double> Solve(Vector<double> location,int maxInterations,double minResidual);
    }
}
