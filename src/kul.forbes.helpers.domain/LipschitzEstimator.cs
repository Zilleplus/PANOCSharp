using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kul.forbes.helpers.domain
{
    public class LipschitzEstimator : ILipschitzEstimator
    {
        private readonly IFunction function;
        private readonly IConfigLipschitzEstimator config;
        private readonly ILogger logger;

        public LipschitzEstimator(
            IFunction function,
            IConfigLipschitzEstimator config,
            ILogger logger)
        {
            this.function = function;
            this.config = config;
            this.logger = logger;
        }

        // Get the step used to estimate the lipschitz constant
        // -> delta= max{small number,10^{-6}*u_0}
        public Vector<double> GetDelta(Vector<double>  location)
        {
            return location.Map((val)=>Math.Max(config.LipschitzSafetyValue*val,config.Delta));
        }

        // Estimate the lipschitz constant by using the numerical hessian as an estimation
        // Theorem:
        //    ||gradient(x)|| < B
        // f is B-lipschitz
        public double Estimate(Vector<double> location)
        {
            var delta = GetDelta(location);
            return (function.Evaluate(location).gradient- function.Evaluate(location+delta).gradient).Norm(2)
                / (delta.Norm(2));
        }
    }
}
