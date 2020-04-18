using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.domain
{
    public static class ProximalGradientStep
    {
        private static bool LineSearchCondition(
            Location location,
            ProxLocation newLocation,
            double safetyValueLineSearch)
        {
            (double f, Vector<double> df) = location.Cost;
            (double fNew, Vector<double> dfNew) = newLocation.Cost;
            var directionSquaredNorm = (location.Position - newLocation.Position)
                .DotProduct(location.Position - newLocation.Position);

            return fNew > f - df.DotProduct(df)
                + (1 - safetyValueLineSearch) / (2 * newLocation.Gamma) * directionSquaredNorm
                + 1e-6 * f;
        }

        public static ProximalGradient Calculate(
            Location location,
            IConfigProximalGradient config,
            IFunction function,
            IProx prox)
        {
            double gamma = location is ProxLocation
                ? (location as ProxLocation).Gamma
                : (1 - config.SafetyValueLineSearch) / LipschitzEstimator.Estimate(location,config,function);

            var newLocation = ProxLocationBuilder.Build(location, gamma,prox,function);
            while (!LineSearchCondition(location, newLocation,config.SafetyValueLineSearch))
            {
                gamma = gamma / 2;
                newLocation = ProxLocationBuilder.Build(location, gamma,prox,function);
            }

            return new ProximalGradient(location,newLocation);
        }
    }
}
