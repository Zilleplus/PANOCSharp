using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.helpers.domain.Accelerators;
using kul.forbes.testTools;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Linq;
using Xunit;

namespace kul.forbes.helpers.test
{
    public class LBFGS_Tests
    {
        Vector<double> RosenBrockGradient(Vector<double> location)
        {
            double a = 1;
            double b = 100;

            var gradient = Vector<double>.Build.Dense(location.Count);

            // Matlab: df = @(x) [-2*(a-(b+1)*x(1)+b*x(2)); 2*b*(x(2)-x(1)) ]; 
            gradient[0] = -2 * (a - (b + 1) * location[0] + b * location[1]);
            gradient[1] = 2 * b * (location[1] - location[0]);

            return gradient;
        }

        class RosenConfig : IConfigLBFGS
        {
            public int CacheSize => 20;

            public int ProblemDimension => 2;
        }

        [Fact]
        public void Given_RosenBrock_Solve_With_LBFGS()
        {
            var rosen = new MockedFunctionBuilder()
                .WithCostGradient(
                    (a)=>0,
                    (x)=> RosenBrockGradient(x))
                .Build()
                .Object;

            var sut = new LBFGS(
                rosen,
                new RosenConfig(),
                default(ILogger));

            var startLocation = new VectorBuilder()
                .WithElements(-1.2, 1)
                .Build();

            var location = startLocation;
            var res = Enumerable
                .Range(0, 4)
                .Select(i =>
                {
                    var newLocation = location + sut.GetStep(location);
                    sut.Update(location, newLocation);
                    newLocation.CopyTo(location);

                    return location.ToArray();
                })
                .ToList();

            var lastElement = res.Last();

            var precision = 0.01;
            Assert.True(Math.Abs(1 - lastElement[0])< precision);
        }
    }
}
