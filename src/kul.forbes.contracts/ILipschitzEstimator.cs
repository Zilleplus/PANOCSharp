using kul.forbes.contracts;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.helpers.contracts
{
    public interface ILipschitzEstimator
    {
        double Estimate(Location location);
    }
}
