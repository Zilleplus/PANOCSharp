using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.domain;
using kul.forbes.helpers.domain.Accelerators;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.panocsharp
{
    /// <summary>
    /// Solver to solve a non-linear optimization problem
    /// </summary>
    public class PANOCSolver
    {
        private readonly IFunction costFunction;
        private readonly IProx proxCostFunction;
        private Panoc solver;

        public PANOCSolver(
            IFunction costFunction, 
            IProx proxCostFunction,
            IConfigPanoc config) 
        {
            solver = new Panoc(new LBFGS(config),config);
            this.costFunction = costFunction;
            this.proxCostFunction = proxCostFunction;
        }

        public double[] Solve(
            double[] startLocation,
            int maxIterations=100,
            double minResidual=1e-3)
            => solver
                .Solve(
                    Vector<double>.Build.DenseOfArray(startLocation),
                    maxIterations,
                    minResidual,
                    costFunction,
                    proxCostFunction)
                .ToArray();
    }
}
