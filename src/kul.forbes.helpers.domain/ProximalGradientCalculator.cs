using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.domain
{
    public class ProximalGradientCalculator : 
        ICalculator<Location, ProximalGradient>
    {
        private readonly IConfigProximalGradient config;
        private readonly ILipschitzEstimator lipschitzEstimator;
        private readonly IBuilder<Location,double, ProxLocation> proxLocationBuilder;

        public ProximalGradientCalculator(
            IConfigProximalGradient config,
            ILipschitzEstimator lipschitzEstimator,
            IBuilder<Location,double,ProxLocation> proxLocationBuilder,
            ILogger logger)
        {
            this.config = config;
            this.lipschitzEstimator = lipschitzEstimator;
            this.proxLocationBuilder = proxLocationBuilder;
        }

        private bool LineSearchCondition(
            Location location,
            ProxLocation newLocation)
        {
            (double f, Vector<double> df) = location.Cost;
            (double fNew, Vector<double> dfNew) = newLocation.Cost;
            var directionSquaredNorm = (location.Position - newLocation.Position)
                .DotProduct(location.Position - newLocation.Position);

            return fNew > f - df.DotProduct(dfNew)
                + (1 - config.SafetyValueLineSearch) / (2 * newLocation.Gamma) * directionSquaredNorm
                + 1e-6 * f;
        }

        public ProximalGradient Calculate(Location location)
        {
            double gamma = location is ProxLocation
                ? (location as ProxLocation).Gamma
                : 1 / lipschitzEstimator.Estimate(location);

            var newLocation = proxLocationBuilder.Build(location, gamma);
            while (!LineSearchCondition(location, newLocation))
            {
                gamma = gamma / 2;
                newLocation = proxLocationBuilder.Build(location, gamma);
            }

            return new ProximalGradient(location,newLocation);
        }
    }
}
