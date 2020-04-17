using Autofac;
using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.domain;
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

        public PANOCSolver(
            IFunction costFunction, 
            IProx proxCostFunction,
            IConfigPanoc config) 
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<PanocModule>();// internal stuff 

            builder.RegisterInstance(config).AsImplementedInterfaces(); // consult docs for details on all the parameters
            builder.RegisterInstance(costFunction).As<IFunction>();
            builder.RegisterInstance(proxCostFunction).As<IProx>();
            if (!config.EnableLogging)
            {
                builder.RegisterInstance(new NothingLogger()).As<ILogger>();
            } // TODO impl a proper logger
            else
            {
                throw new NotImplementedException();
            }

            var container = builder.Build();
            solver = container.Resolve<IPanoc>();
        }

        public double[] Solve(double[] startLocation,int maxIterations=100,double minResidual=1e-3)
            => solver
                .Solve(Vector<double>.Build.DenseOfArray(startLocation),maxIterations,minResidual)
                .ToArray();
    }
}
