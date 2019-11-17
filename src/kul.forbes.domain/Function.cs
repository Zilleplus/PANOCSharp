using kul.forbes.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.domain
{
    public class Function : IFunction
    {
        private readonly Func<double[], (double, double[])> func;

        public Function(
            Func<double[],(double,double[])> func)
        {
            this.func = func;
        }

        public (double cost, Vector<double> gradient) Evaluate(Vector<double> location)
        {
            var (cost, gradient) = func(location.AsArray());

            return (cost, Vector<double>.Build.Dense(gradient));
        }
    }
}
