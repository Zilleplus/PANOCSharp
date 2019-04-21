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
            var degree = 2;
            var poly = new MockedFunctionBuilder()
                .WithCostGradient(
                    (x) => x.PointwisePower(degree).Sum(),
                    (x) => degree*x.PointwisePower(degree-1))
                .Build()
                .Object;
            var proximalOperator2 = new ProxBox(10,1e7,2);
            var proximalOperator = new NormBox(dimension: 2, penality: 1e7, offSet: 2);
            var proximalBuilder = new ProxLocationBuilder(poly, proximalOperator);
            var locationBuilder = new LocationBuilder(poly);

            var sut = new ProximalGradientCalculator(
                new ProxConfig(),
                new LipschitzEstimator(poly, new ProxConfig(), default(ILogger)),
                proximalBuilder,
                default(ILogger));

            var init = locationBuilder.Build(0.5, 0.5);
            var location = sut.Calculate(init).ProxLocation;
            var loops = Enumerable.Range(0, numberOfIterations)
                .Select(i=> 
                {
                    location = sut.Calculate(location).ProxLocation;
                    return location.Position.ToArray();
                })
                .ToList();

            var expected = Vector<double>.Build.Dense(new[] { 0.0, 0.0});

            var res = expected.Zip(loops.Last(), (expect, actual) => (expect, actual));
            res.ForEach(t=> Assert.Equal(expected: t.expect, actual: t.actual, precision: 2));
        }
    }
}
