using System;
using kul.forbes;
using kul.forbes.panocsharp;
using MathNet.Numerics.LinearAlgebra;

namespace Rosenbrock
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Demo of PANOC API");

            var degree = 2;
            var costFunction = new VectorFunction(x => (x.PointwisePower(degree).Sum(), degree * x.PointwisePower(degree - 1)));
            var constraint = new ProxBox(size: 1, penalty: 1, dimension: 2);
            var defaultConfig = new ConfigPanoc(problemDimension: 2);

            var solver = new PANOCSolver(costFunction,constraint,defaultConfig);
            var solution = solver.Solve(new double[] { 1.0,1.0 });

            Console.WriteLine($"Solved with solution: [{solution[0]};{solution[1]}]");
        }
    }
}
