using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.helpers.domain.Accelerators
{
    public class LBFGS : IAccelerator
    {
        private readonly IFunction function;
        private readonly IConfigLBFGS config;

        private Vector<double> rho;
        private Vector<double> alpha;

        private Matrix<double> s;
        private Matrix<double> y;

        private int activeBufferSize;
        private int cursor;

        public LBFGS(
            IFunction function,
            IConfigLBFGS config)
        {
            this.function = function;
            this.config = config;

            s = Matrix<double>.Build.Dense(config.ProblemDimension,config.CacheSize);
            y = Matrix<double>.Build.Dense(config.ProblemDimension,config.CacheSize);

            rho = Vector<double>.Build.Dense(config.CacheSize);
            alpha = Vector<double>.Build.Dense(config.CacheSize);
            activeBufferSize = 0;
            cursor = 0;
        }

        public Vector<double> GetStep(Vector<double> location)
        {
            if (activeBufferSize == 0) return -function.Evaluate(location).gradient;

            for (int i = 0; i < activeBufferSize; i++)
            {
                var floatIndex = GetFloatingIndex(i);
            }


            throw new NotImplementedException();
        }

        private int GetFloatingIndex(int i)
            => (cursor - 1 - i + config.CacheSize) % config.CacheSize;

        public void Update(Vector<double> location, Vector<double> newLocation)
        {
            throw new NotImplementedException();
        }
    }
}
