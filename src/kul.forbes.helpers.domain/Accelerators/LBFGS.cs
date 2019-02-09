using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;
using MoreLinq;
using System;
using System.Linq;

namespace kul.forbes.helpers.domain.Accelerators
{
    public class LBFGS : IAccelerator
    {
        private readonly IFunction function;
        private readonly IConfigLBFGS config;
        private readonly ILogger logger;
        private Matrix<double> s;
        private Matrix<double> y;

        private int activeCacheSize;
        private int cursor;

        double hessianEstimate;

        public LBFGS(
            IFunction function,
            IConfigLBFGS config,
            ILogger logger)
        {
            this.function = function;
            this.config = config;
            this.logger = logger;
            s = Matrix<double>.Build.Dense(config.ProblemDimension,config.CacheSize);
            y = Matrix<double>.Build.Dense(config.ProblemDimension,config.CacheSize);
            
            activeCacheSize = 0;
            cursor = 0;
        }

        public Vector<double> GetStep(Vector<double> location)
        {
            if (activeCacheSize == 0) return -function.Evaluate(location).gradient;

            var outputDirection = function.Evaluate(location).gradient;
            var rho = Vector<double>.Build.Dense(config.CacheSize);
            var alpha = Vector<double>.Build.Dense(config.CacheSize);

            Enumerable.Range(0, activeCacheSize)
                .Select(GetFloatingIndex)
                .ToList()
                .ForEach(i => 
                {
                    rho[i] = 1 / (s.Column(i).DotProduct(y.Column(i)));
                    alpha[i] = rho[i] * (s.Column(i).DotProduct(outputDirection));

                    outputDirection = outputDirection - (alpha[i] * y.Column(i));
                });

            outputDirection = outputDirection * hessianEstimate;

            Enumerable.Range(0, activeCacheSize)
                .Reverse()
                .Select(GetFloatingIndex)
                .ToList()
                .ForEach(i=>
                {
                    var beta = rho[i] * (y.Column(i).DotProduct(outputDirection));
                    outputDirection = outputDirection + ((alpha[i] - beta) * s.Column(i));
                });

            return -outputDirection;
        }

        private int GetFloatingIndex(int i)
            => (cursor - 1 - i + config.CacheSize) % config.CacheSize;

        public bool Update(Vector<double> location, Vector<double> newLocation)
        {
            Func<Vector<double>, Vector<double>> df = (loc) => function.Evaluate(loc).gradient;
            var potentialS = newLocation - location;
            var potentialY = df(newLocation) - df(location);

            var safetyValueCarefullUpdate = potentialS.DotProduct(potentialY) / potentialS.DotProduct(potentialS);
            if (safetyValueCarefullUpdate > df(location).Norm(2) * 1e-12)
            {
                potentialS.CopyTo(s.Column(cursor));
                potentialY.CopyTo(y.Column(cursor));
                hessianEstimate = potentialS.DotProduct(potentialY) / potentialY.DotProduct(potentialY);

                activeCacheSize = (activeCacheSize < config.CacheSize)
                ? activeCacheSize + 1 : activeCacheSize;
                cursor = (cursor + 1) % config.CacheSize;

                return true;
            }
            return false;
        }
    }
}
