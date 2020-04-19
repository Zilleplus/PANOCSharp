using kul.forbes.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes
{
    public class VectorFunction : IFunction
    {
        private readonly Func<Vector<double>, (double, Vector<double>)> costFunction;

        public VectorFunction(Func<Vector<double>,(double,Vector<double>)> costFunction)
        {
            this.costFunction = costFunction;
        }

        public (double cost, Vector<double> gradient) Evaluate(Vector<double> position)
            => costFunction(position);
    }

    public class Function : IFunction
    {
        private readonly Func<double[], (double, double[])> func;

        public Function(
            Func<double[],(double,double[])> func)
        {
            this.func = func;
        }

        public (double cost, Vector<double> gradient) Evaluate(Vector<double> position)
        {
            var (cost, gradient) = func(position.AsArray());

            return (cost, Vector<double>.Build.Dense(gradient));
        }
    }
}
