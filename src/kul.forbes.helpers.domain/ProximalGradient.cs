using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.domain
{
    public class ProximalGradient : IProximalGradient
    {
        private readonly IConfigProximalGradient config;
        private readonly ILipschitzEstimator lipschitzEstimator;
        private readonly IFunction function;
        private readonly IProx prox;

        public double Gamma { get; private set; }

        public int Iteration { get; private set; }

        public ProximalGradient(
            IConfigProximalGradient config,
            ILipschitzEstimator lipschitzEstimator,
            IFunction function,
            IProx prox,
            ILogger logger)
        {
            this.config = config;
            this.lipschitzEstimator = lipschitzEstimator;
            this.function = function;
            this.prox = prox;

            Iteration = 0;
        }

        public Vector<double> GetStep(Vector<double> location)
        {
            Gamma = (Iteration != 0) ? Gamma : 1 / lipschitzEstimator.Estimate(location);
            Iteration++;
            
            return LineSearch(location);
        }

        public Vector<double> LineSearch(Vector<double> location)
        {
            var newLocation = Vector<double>.Build.Dense(location.Count);

            DoProximalStep(newLocation);
            while (!LineSearchCondition(location, newLocation)) 
            {
                DoProximalStep(newLocation);
            } 

            return newLocation;
        }

        private bool LineSearchCondition(
            Vector<double> location,
            Vector<double> newLocation)
        {
            (double f, Vector<double> df) = function.Evaluate(location);
            (double fNew, Vector<double> dfNew) = function.Evaluate(newLocation);

            var directionSquaredNorm = (location - newLocation).DotProduct(location - newLocation);

            return fNew > f - df.DotProduct(dfNew)
                + (1 - config.SafetyValueLineSearch) / (2 * Gamma) * directionSquaredNorm
                + 1e-6 * f;
        }

        public double DoProximalStep(Vector<double> location)
        {
            return  prox.Prox(location - Gamma * function.Evaluate(location).gradient);
        }
    }
}
