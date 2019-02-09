using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.domain
{
    public class Panoc : IPanoc
    {
        private readonly IConfigPanoc config;
        private readonly ILogger logger;

        public Panoc(
            IConfigPanoc config,
            ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public Vector<double> Solve(Vector<double> location)
        {
            throw new NotImplementedException();
        }
    }
}
