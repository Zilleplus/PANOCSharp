using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.domain;
using kul.forbes.helpers.domain;
using kul.forbes.testTools;
using MathNet.Numerics.LinearAlgebra;
using MoreLinq;
using System.Linq;
using Xunit;

namespace kul.forbes.helpers.test
{
    public class ProximalGradientTests
    {
        class ProxConfig : IConfigProximalGradient
        {
            public double SafetyValueLineSearch => 0.05;

            public double LipschitzSafetyValue => 1e-6;

            public double Delta => 1e-12;
        }

        class LipConfig : IConfigLipschitzEstimator
        {
            public double LipschitzSafetyValue => 1e-6;

            public double Delta => 1e-12;
        }

        [Fact]
        public void Given_Polynomial_2th_degree_Solve()
        {
            var numberOfIterations = 100;
            var degree = 5;
            var poly = new MockedFunctionBuilder()
                .WithCostGradient(
                    (x) => x.PointwisePower(degree).Sum(),
                    (x) => degree*x.PointwisePower(degree-1))
                .Build()
                .Object;
            var proximalOperator = new NormBox(dimension: 2, penality: 1e10, offSet: 2);
            var locationBuilder = new LocationBuilder(poly);

            var sut = new ProximalGradientCalculator(
                new ProxConfig(),
                new LipschitzEstimator(poly, new ProxConfig(), default(ILogger)),
                new ProxLocationBuilder(poly, proximalOperator),
                default);

            var init = locationBuilder.Build(0.5, 0.5);
            var location = sut.Calculate(init).ProxLocation;
            var loops = Enumerable.Range(0, numberOfIterations)
                .Select(i=> 
                {
                    location = sut.Calculate(location).ProxLocation;
                    return location.Position.ToArray();
                })
                .ToList();

            var expected = Vector<double>.Build.Dense(new[] { 0.11881573667192692, 0.11881573667192692 }); // answers taken from the C-code

            Assert.Equal(expected: expected[0], actual: loops.Last()[0], precision: 8);
            Assert.Equal(expected: expected[1], actual: loops.Last()[1], precision: 8);
        }
    }
}
