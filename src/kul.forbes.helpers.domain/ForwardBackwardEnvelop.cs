using kul.forbes.contracts.configs;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.helpers.domain
{
    public class ForwardBackwardEnvelop : IForwardBackwardEnvelop
    {
        private readonly IProximalGradient proximalGradient;
        private readonly IConfigForwardBackwardEnvelop config;

        public ForwardBackwardEnvelop(
            IProximalGradient proximalGradient,
            IConfigForwardBackwardEnvelop config)
        {
            this.proximalGradient = proximalGradient;
            this.config = config;
        }

        double CalculateEnvelop(Vector<double> location)
        {
            throw new NotImplementedException();
        }
    }
}
