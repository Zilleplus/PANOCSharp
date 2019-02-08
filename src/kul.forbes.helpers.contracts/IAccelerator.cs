using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.helpers.contracts
{
    public interface IAccelerator
    {
        Vector<double> GetStep(Vector<double> location);

        void Update(Vector<double> location, Vector<double> newLocation);
    }
}
