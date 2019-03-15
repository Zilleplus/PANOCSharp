using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.helpers.contracts
{
    public interface IAccelerator
    {
        Vector<double> GetStep(Location location);

        bool Update(Location oldLocation,Location newLocation);
    }
}
