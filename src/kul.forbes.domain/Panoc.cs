using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.domain
{
    public class Panoc : IPanoc
    {
        private readonly IConfigPanoc config;

        public Panoc(
            IConfigPanoc config)
        {
            this.config = config;
        }

        public Vector<double> Solve(Vector<double> location)
        {
            throw new NotImplementedException();
        }
    }
}
