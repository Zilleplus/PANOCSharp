using Autofac;
using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.IoC;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.API
{
    /// <summary>
    /// Solver to solve a non-linear optimization problem
    /// </summary>
    public class PANOCSolver
    {
        private IPanoc solver;

        public PANOCSolver(IConfigPanoc config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<PanocModule>();

            // TODO register cost/gradient, configuration and prox constraint

            var container = builder.Build();

            solver = container.Resolve<IPanoc>();
        }

        public double[] Solve(double[] startLocation)
        {
            return solver.Solve(Vector<double>.Build.DenseOfArray(startLocation)).ToArray();
        }
    }
}
