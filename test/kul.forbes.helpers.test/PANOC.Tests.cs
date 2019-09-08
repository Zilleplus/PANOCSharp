using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.helpers.domain;
using kul.forbes.helpers.domain.Accelerators;
using kul.forbes.testTools;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace kul.forbes.helpers.test
{
    public class PANOC_Tests
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

        double RosenBrockCost(Vector<double> location)
        {
            double a = 1;
            double b = 100;

            // Matlab: f =@(x) (a-x(1))^2 + b*(x(2)-x(1))^2; 
            return Math.Pow((a - x[0]), 2) + b * Math.Pow((x[1] - x[0]), 2);
        }

        class PanocConfig : IConfigPanoc
        {
            public int CacheSize => 20;

            public int ProblemDimension => 2;
        }

        [Fact]
        public void Given_Polynomial_2th_degree_Solve()
        {
            var numberOfIterations = 99;
            var degree = 5;
            var poly = new MockedFunctionBuilder()
                .WithCostGradient(
                    (x) => x.PointwisePower(degree).Sum(),
                    (x) => degree * x.PointwisePower(degree - 1))
                .Build()
                .Object;
            var proximalOperator = new NormBox(dimension: 2, penality: 1e10, offSet: 2);
            var locationBuilder = new LocationBuilder(poly);

            var sut = new ProximalGradientCalculator(
                new ProxConfig(),
                new LipschitzEstimator(poly, new ProxConfig(), default),
                new ProxLocationBuilder(poly, proximalOperator),
                default);

            var init = locationBuilder.Build(0.5, 0.5);
            var location = sut.Calculate(init).ProxLocation;
            var loops = Enumerable.Range(0, numberOfIterations)
                .Select(i =>
                {
                    location = sut.Calculate(location).ProxLocation;
                    return location.Position.ToArray();
                })
                .ToList();

            var expected = Vector<double>.Build.Dense(new[] { 0.118816, 0.118816 }); // answers taken from the C-code

            Assert.Equal(expected: expected[0], actual: loops.Last()[0], precision: 6);
            Assert.Equal(expected: expected[1], actual: loops.Last()[1], precision: 6);
        }
    }
}
