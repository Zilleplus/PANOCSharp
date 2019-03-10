using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.domain
{
    public class ForwardBackwardEnvelopCalculator : ICalculator<ProximalGradient,double>
    {
        private readonly ICalculator<Location, ProxLocation> proxCalculator;
        private readonly IConfigForwardBackwardEnvelop config;
        private readonly ICalculator<ProximalGradient, Vector<double>> residualCalculator;

        public ForwardBackwardEnvelopCalculator(
            ICalculator<Location,ProxLocation> proxCalculator,
            IConfigForwardBackwardEnvelop config,
            ICalculator<ProximalGradient, Vector<double>> residualCalculator,
            ILogger logger)
        {
            this.proxCalculator = proxCalculator;
            this.config = config;
            this.residualCalculator = residualCalculator;
        }

        /*
         * calculate the forward backward envelop using the internal gamma
         * Matlab cache.FBE = cache.fx + cache.gz - cache.gradfx(:)'*cache.FPR(:)
         * + (0.5/gam)*(cache.normFPR^2);
         */
        public double Calculate(ProximalGradient proxGradient)
        {
            (var fx, var df) = proxGradient.Location.Cost;

            return fx
                + proxGradient.ProxLocation.Constraint.Cost
                - df.DotProduct(residualCalculator.Calculate(proxGradient))
                + (1 / (proxGradient.ProxLocation.Gamma * 2));
        }
    }
}
