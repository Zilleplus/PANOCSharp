using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.helpers
{
    public class LipschitzEstimator
    {
        // Get the step used to estimate the lipschitz constant
        // -> delta= max{small number,10^{-6}*u_0}
        public static Vector<double> GetDelta(Vector<double>  location, IConfigLipschitzEstimator config)
            => location.Map((val)=>Math.Max(config.LipschitzSafetyValue*val,config.MinimumDelta));

        // Estimate the lipschitz constant by using the numerical hessian as an estimation
        // Theorem:
        //    ||gradient(x)|| < B
        // f is B-lipschitz
        public static double Estimate(Location location,IConfigLipschitzEstimator config, IFunction function)
        {
            var delta = GetDelta(location.Position,config);
            return (location.Evaluated.Gradient- function.Evaluate(location.Position+delta).gradient).Norm(2)
                / ((delta).Norm(2));
        }
    }
}
