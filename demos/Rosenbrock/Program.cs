using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using kul.forbes.API;
using kul.forbes.contracts;
using kul.forbes.IoC;

namespace Rosenbrock
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Demo of PANOC API");

            var solver = new PANOCSolver(default);

            solver.Solve(new double[] { 1.0,1.0 });
        }
    }
}
