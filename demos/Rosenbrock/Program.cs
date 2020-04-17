using System;
using kul.forbes.API;
using kul.forbes.domain;

namespace Rosenbrock
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Demo of PANOC API");

            var costFunction = new Function((a) => (0, a));
            var constraint = new ProxBox(size: 1, penalty: 1, dimension: 2);
            var defaultConfig = new ConfigPanoc(problemDimension: 2);

            var solver = new PANOCSolver(costFunction,constraint,defaultConfig);

            var solution = solver.Solve(new double[] { 1.0,1.0 });

            Console.WriteLine($"Solved with solution: [{solution[0]};{solution[1]}]");
        }
    }
}
