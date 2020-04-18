using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;
using MoreLinq;
using System;
using System.Linq;

namespace kul.forbes.helpers.domain.Accelerators
{
    public class LBFGS : IAccelerator
    {
        private readonly IConfigLBFGS config;
        private Matrix<double> s;
        private Matrix<double> y;

        private int activeCacheSize;
        private int cursor;

        double hessianEstimate;

        public LBFGS( IConfigLBFGS config )
        {
            this.config = config;
            s = Matrix<double>.Build.Dense(config.ProblemDimension,config.CacheSize);
            y = Matrix<double>.Build.Dense(config.ProblemDimension,config.CacheSize);
            
            activeCacheSize = 0;
            cursor = 0;
        }

        public bool HasCache { get { return activeCacheSize != 0; }  }

        /// <summary>
        /// Calculates to step towards the minima
        /// --> if no updates have been made, it returns -gradient
        /// </summary>
        public Vector<double> GetStep(Location location)
        {
            if (activeCacheSize == 0) return -location.Cost.Gradient;

            var outputDirection = location.Cost.Gradient;
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

        public bool Update(Location oldLocation,Location newLocation)
        {
            var potentialS = newLocation.Position - oldLocation.Position;
            var potentialY = newLocation.Cost.Gradient - oldLocation.Cost.Gradient;

            var safetyValueCarefullUpdate = potentialS.DotProduct(potentialY) / potentialS.DotProduct(potentialS);
            if (safetyValueCarefullUpdate > oldLocation.Cost.Gradient.Norm(2) * 1e-12)
            {
                for (int i = 0; i < s.RowCount; i++)
                {
                    s[i, cursor] = potentialS[i];
                    y[i, cursor] = potentialY[i];
                }
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
