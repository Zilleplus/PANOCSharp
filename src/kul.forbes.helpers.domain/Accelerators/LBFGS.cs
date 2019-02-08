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

        private Matrix<double> s;
        private Matrix<double> y;

        private int activeBufferSize;
        private int cursor;

        double hessianEstimate;

        public LBFGS(
            IFunction function,
            IConfigLBFGS config)
        {
            this.function = function;
            this.config = config;

            s = Matrix<double>.Build.Dense(config.ProblemDimension,config.CacheSize);
            y = Matrix<double>.Build.Dense(config.ProblemDimension,config.CacheSize);

            
            activeBufferSize = 0;
            cursor = 0;
        }

        public Vector<double> GetStep(Vector<double> location)
        {
            if (activeBufferSize == 0) return -function.Evaluate(location).gradient;

            var outputDirection = function.Evaluate(location).gradient;
            var rho = Vector<double>.Build.Dense(config.CacheSize);
            var alpha = Vector<double>.Build.Dense(config.CacheSize);

            Enumerable.Range(0, activeBufferSize)
                .Select(GetFloatingIndex)
                .ForEach(i => 
                {
                    rho[i] = 1 / (s.Column(i).DotProduct(y.Column(i)));
                    alpha[i] = rho[i] * (s.Column(i).DotProduct(outputDirection));

                    outputDirection = outputDirection - (alpha[i] * y.Column(i));
                });

            outputDirection = outputDirection * hessianEstimate;

            Enumerable.Range(0, activeBufferSize)
                .Reverse()
                .Select(GetFloatingIndex)
                .ForEach(i=>
                {
                    var beta = rho[i] * (y.Column(i).DotProduct(outputDirection));
                    outputDirection = outputDirection + ((alpha[i] - beta) * s.Column(i));
                });

            return -outputDirection;
        }

        private int GetFloatingIndex(int i)
            => (cursor - 1 - i + config.CacheSize) % config.CacheSize;

        public void Update(Vector<double> location, Vector<double> newLocation)
        {
            throw new NotImplementedException();
        }
    }
}
