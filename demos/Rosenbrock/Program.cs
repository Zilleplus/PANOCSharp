using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using kul.forbes.contracts;
using kul.forbes.IoC;

namespace Rosenbrock
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Demo of PANOC using rosenbrock function");

            var container = SetupContainer();

            var solver = container.Resolve<IPanoc>();
        }

        private static IContainer SetupContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<PanocModule>();

            return builder.Build();
        }
    }
}
